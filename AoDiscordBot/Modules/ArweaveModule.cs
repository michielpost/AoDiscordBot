using AoDiscordBot.Models;
using AoDiscordBot.Services;
using ArweaveAO;
using ArweaveAO.Models;
using ArweaveAO.Models.Token;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AoDiscordBot.Modules;

public class ArweaveModule : ModuleBase<SocketCommandContext>
{
    private const string CRED = "Sa0iBLPNyJQrwpTTG-tWLQU-1QeUAJA73DdxGGiKoJc";
    private const string AO = "m3PaWzK4PTG9lAaqYQPaPdOcXdO8hYqi5Fe9NWqXd0w";
    private const string AOPROXY = "Pi-WmAQp2-mh-oWH9lWpz5EthlUDj_W0IusAv-RXhRk";

    public TokenClient tokenClient = new TokenClient(Options.Create(new ArweaveConfig()), new HttpClient());
    public GraphqlClient graphqlClient = new GraphqlClient(new HttpClient(), Options.Create<GraphqlConfig>(new()));
    public TokenDataService tokenDataService = new TokenDataService(Options.Create<ArweaveConfig>(new()), new());

    [Command("balance")]
    public async Task Balance(params string[] objects)
    {
        if (objects.Length == 0)
        {
            await ReplyAsync($"Please supply an address and optionally a token id.");
        }

        var address = objects[0];
        string tokenId = null;
        if (objects.Length > 1)
            tokenId = objects[1];

        bool isValid = await ValidateAddress(address);
        if (!isValid) return;

        if (string.IsNullOrWhiteSpace(tokenId))
            tokenId = AOPROXY;

        TokenData? tokenByName = await tokenDataService.GetTokenInfoByName(tokenId);
        if (tokenByName == null)
        {
            bool isValidTokenId = await ValidateAddress(tokenId);
            if (!isValidTokenId) return;
        }

        TokenData? tokenInfo = await tokenDataService.GetTokenInfo(tokenId) ?? tokenByName;
        if (tokenInfo == null)
        {
            await ReplyAsync("Unable to get token info for this token.");
            return;
        }

        var result = await tokenClient.GetBalance(tokenInfo.TokenId, address);

        var formattedBalance = BalanceHelper.FormatBalance(result?.Balance, tokenInfo?.Denomination ?? 0);

        var reply = new StringBuilder();
        reply.AppendLine($"Address: {address}");
        reply.AppendLine($"Balance: {formattedBalance} {tokenInfo?.Ticker}");

        await ReplyAsync(reply.ToString());
    }



    [Command("transactions")]
    public async Task Transactions([Remainder] string? address = null)
    {
        bool isValid = await ValidateAddress(address);
        if (!isValid) return;

        var incoming = await graphqlClient.GetTokenTransfersIn(address);
        var outgoing = await graphqlClient.GetTransactionsOut(address);
        var outgoingProcess = await graphqlClient.GetTransactionsOutFromProcess(address);

        var allNew = incoming.Concat(outgoing).Concat(outgoingProcess).OrderByDescending(x => x.Timestamp).ToList();

        if (!allNew.Any())
        {
            await ReplyAsync($"No transactions found for address: {address}");
            return;
        }

        var showTrans = allNew.Take(5).ToList();

        var reply = new StringBuilder();
        reply.AppendLine($"Last {showTrans.Count} transactions for {address}");
        foreach (var transaction in showTrans)
        {
            var ticker = await GetTokenTicker(transaction.TokenId);
            reply.AppendLine($"{transaction.Quantity} {ticker} | {transaction.From} > {transaction.To} | {transaction.Id}");
        }

        reply.AppendLine("Reply with 'transaction {txId}' to receive more information about a transaction.");

        var builder = new ComponentBuilder();
        builder = builder.WithButton("View on ao.link", style: ButtonStyle.Link, url: $"https://www.ao.link/#/entity/{address}");


        await ReplyAsync(reply.ToString(), components: builder.Build());
    }

    private async Task<string?> GetTokenTicker(string? tokenId)
    {
        if (string.IsNullOrWhiteSpace(tokenId))
            return "Unknown token";

        var tokenMeta = await tokenDataService.GetTokenInfo(tokenId);
        return tokenMeta?.Ticker;
    }

    [Command("transaction")]
    public async Task Transaction([Remainder] string? txId = null)
    {
        bool isValid = await ValidateAddress(txId);
        if (!isValid) return;

        var result = await graphqlClient.GetTransactionsById(txId);

        if (result == null || string.IsNullOrEmpty(result.Id))
        {
            await ReplyAsync($"Transaction not found. TxId:: {txId}");
            return;
        }

        var builder = new ComponentBuilder();
        builder = builder.WithButton("View on ao.link", style: ButtonStyle.Link, url: $"https://www.ao.link/#/message/{txId}");

        var reply = new StringBuilder();
        reply.AppendLine($"Transaction info: {txId}");
        reply.AppendLine($"{result.Timestamp}");
        reply.AppendLine($"From: {result.From} | To: {result.To}");
        reply.AppendLine($"{result.TokenTransferType} Amount: {result.Quantity} | Token: {await GetTokenTicker(result.TokenId)}");

        await ReplyAsync(reply.ToString(), components: builder.Build() );
    }

    [Command("token")]
    public async Task Token([Remainder] string? tokenId = null)
    {
        
        if (string.IsNullOrEmpty(tokenId))
        {
            await ReplyAsync($"Please provide a token id.");
            return;
        }

        TokenData? tokenByName = await tokenDataService.GetTokenInfoByName(tokenId);
        if (tokenByName == null)
        {
            bool isValidTokenId = await ValidateAddress(tokenId);
            if (!isValidTokenId) return;
        }

        TokenData? tokenInfo = await tokenDataService.GetTokenInfo(tokenId);
        if (tokenInfo == null)
        {
            await ReplyAsync($"Unable to get token data for: {tokenId}");
            return;
        }

        var reply = new StringBuilder();
        reply.AppendLine($"Token info: {tokenInfo.TokenId}");
        reply.AppendLine($"Name: {tokenInfo.Name}");
        reply.AppendLine($"Ticker: {tokenInfo.Ticker}");
        reply.AppendLine($"Denomination: {tokenInfo.Denomination}");
        //reply.AppendLine($"Logo: {tokenInfo.Logo}");

        await ReplyAsync(reply.ToString());

    }

    [Command("actions")]
    public async Task Actions([Remainder] string? address = null)
    {
        bool isValid = await ValidateAddress(address);
        if (!isValid) return;

        var actions = await graphqlClient.GetActionsForProcess(address);
        if (actions == null || !actions.Any())
            await ReplyAsync($"No actions found for: {address}");

        var builder = new ComponentBuilder();

        foreach (var action in actions)
        {
            builder = builder.WithButton(action.Name, style: ButtonStyle.Link, url: $"https://aoww.net/action-builder?processId={address}&actionName={action.Name}");
        }

        await ReplyAsync($"Possible actions for: {address}", components: builder.Build());

    }


    private async Task<bool> ValidateAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            await ReplyAsync($"Sorry, can't help you. Please provide a valid address.");
            return false;
        }

        if (!AddressValidator.IsValidAddress(address))
        {
            await ReplyAsync($"Sorry, can't help you. It looks like {address} is not a valid address.");
            return false;
        }

        return true;
    }

    [Command("help")]
    public async Task Help()
    {
        var reply = new StringBuilder();
        reply.AppendLine("**Available Commands:**");
        reply.AppendLine();

        reply.AppendLine("1. **balance [address] [token_id]**");
        reply.AppendLine("   - Get the balance of an address for a specific token.");
        reply.AppendLine("   - Input: Address (required), Token ID (optional, defaults to AO Token)");
        reply.AppendLine("   - Output: Address and balance information");
        reply.AppendLine();

        reply.AppendLine("2. **transactions [address]**");
        reply.AppendLine("   - List recent transactions for an address.");
        reply.AppendLine("   - Input: Address (required)");
        reply.AppendLine("   - Output: List of recent transactions");
        reply.AppendLine();

        reply.AppendLine("3. **transaction [tx_id]**");
        reply.AppendLine("   - Get detailed information about a specific transaction.");
        reply.AppendLine("   - Input: Transaction ID (required)");
        reply.AppendLine("   - Output: Detailed transaction information");
        reply.AppendLine();

        reply.AppendLine("4. **token [token_id]**");
        reply.AppendLine("   - Get information about a specific token.");
        reply.AppendLine("   - Input: Token ID (required)");
        reply.AppendLine("   - Output: Token information (name, ticker, denomination)");
        reply.AppendLine();

        reply.AppendLine("5. **actions [address]**");
        reply.AppendLine("   - List possible actions for a given process address.");
        reply.AppendLine("   - Input: Process Address (required)");
        reply.AppendLine("   - Output: List of possible actions with clickable links");
        reply.AppendLine();

        reply.AppendLine("6. **help**");
        reply.AppendLine("   - Display this help message with all available commands.");
        reply.AppendLine("   - Input: None");
        reply.AppendLine("   - Output: List of all commands and their usage");

        await ReplyAsync(reply.ToString());
    }

}
