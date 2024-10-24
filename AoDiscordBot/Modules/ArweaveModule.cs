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

        bool isValidTokenId = await ValidateAddress(tokenId);
        if (!isValidTokenId) return;

        TokenData? tokenInfo = await tokenDataService.GetTokenInfo(tokenId);
        if (tokenInfo == null)
        {
            await ReplyAsync("Unable to get token info for this token.");
            return;
        }

        var result = await tokenClient.GetBalance(tokenId, address);

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

        var reply = new StringBuilder();
        reply.AppendLine($"Last {allNew.Count} transactions for {address}");
        foreach (var transaction in allNew)
        {
            reply.AppendLine($"{transaction.Quantity} {GetTokenTicker(transaction.TokenId)} | {transaction.From} > {transaction.To} | {transaction.Id}");
        }

        reply.AppendLine("Reply with 'transaction {txId}' to receive more information about a transaction.");


        await ReplyAsync(reply.ToString());
    }

    private async Task<string?> GetTokenTicker(string? tokenId)
    {
        if (string.IsNullOrWhiteSpace(tokenId))
            return "Unkwon token";

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

        var reply = new StringBuilder();
        reply.AppendLine($"Transaction info: {txId}");
        reply.AppendLine($"{result.Timestamp}");
        reply.AppendLine($"From: {result.From} | To: {result.To}");
        reply.AppendLine($"{result.TokenTransferType} Amount: {result.Quantity} | Token: {GetTokenTicker(result.TokenId)}");

        await ReplyAsync(reply.ToString());
    }

    [Command("token")]
    public async Task Token([Remainder] string? tokenId = null)
    {
        bool isValid = await ValidateAddress(tokenId);
        if (!isValid) return;

        if (string.IsNullOrEmpty(tokenId))
        {
            await ReplyAsync($"Please provide a token id.");
            return;
        }

        TokenData? tokenInfo = await tokenDataService.GetTokenInfo(tokenId);
        if (tokenInfo == null)
        {
            await ReplyAsync($"Unable to get token data for: {tokenId}");
            return;
        }

        var reply = new StringBuilder();
        reply.AppendLine($"Token info: {tokenId}");
        reply.AppendLine($"Name: {tokenInfo.Name}");
        reply.AppendLine($"Ticker: {tokenInfo.Ticker}");
        reply.AppendLine($"Denomination: {tokenInfo.Denomination}");
        reply.AppendLine($"Logo: {tokenInfo.Logo}");

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



}
