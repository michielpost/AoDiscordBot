# AoDiscordBot
Discord Bot for Arweave AO

## Install
Add the bot to your server:  
https://discord.com/oauth2/authorize?client_id=1299023316157337712&permissions=0&integration_type=0&scope=bot

## Example Commands
- `balance aGeRSnWykicBEGESPbTXPQ0_q2IiMLBBMyemu2pBYoA KmGmJieqSRJpbW6JJUFQrH3sQPEG9F6DQETlXNt4GpM`
- `token KmGmJieqSRJpbW6JJUFQrH3sQPEG9F6DQETlXNt4GpM`
- `transactions aGeRSnWykicBEGESPbTXPQ0_q2IiMLBBMyemu2pBYoA`
- `transaction 2sZkXP3mZyl6ZvILZwZ_xHiBwuwlCowlN4gkqkGcMVA`
- `actions KmGmJieqSRJpbW6JJUFQrH3sQPEG9F6DQETlXNt4GpM`

## Commands Documentation
```
balance [address] [token_id]
Get the balance of an address for a specific token.
Input: Address (required), Token ID (optional, defaults to AO Token)
Output: Address and balance information

transactions [address]
List recent transactions for an address.
Input: Address (required)
Output: List of recent transactions

transaction [tx_id]
Get detailed information about a specific transaction.
Input: Transaction ID (required)
Output: Detailed transaction information

token [token_id]
Get information about a specific token.
Input: Token ID (required)
Output: Token information (name, ticker, denomination)

actions [address]
List possible actions for a given process address.
Input: Process Address (required)
Output: List of possible actions with clickable links

help
Display this help message with all available commands.
Input: None
Output: List of all commands and their usage
```

## Hosting
Hosting is done on Azure, URL: https://aodiscordbot.azurewebsites.net
