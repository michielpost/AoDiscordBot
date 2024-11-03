# AO Discord Bot
Discord Bot for Arweave AO

The AO Discord bot was created for the [PermaHacks 2024](https://www.weaversofficial.com/) hackathon. This bot allows users to interact with the AO network through simple, easy-to-use commands when they are using Discord. 

Functionality:
- Check the balance of any address for a specific token
- View recent transactions for a given address
- Retrieve detailed information about a specific transaction
- Get information about a specific token, including its name, ticker, and denomination
- Explore possible actions for a given process address

To get started, simply add the bot to your server using the provided [link](https://discord.com/oauth2/authorize?client_id=1299023316157337712&permissions=0&integration_type=0&scope=bot), or join the [AO Bot Test Server](https://discord.gg/3X5Eaf3dc7) to see the bot in action.

## Install
Add the bot to your server:  
https://discord.com/oauth2/authorize?client_id=1299023316157337712&permissions=0&integration_type=0&scope=bot

## Live Demo
Add the bot to your own server, or join this test server to talk to the bot!  
- Join the AO Bot Test Server: https://discord.gg/3X5Eaf3dc7
- Start a conversation with AO Bot in a DM or in the main channel using `@AO Bot help`


## Example Commands
- `help`
- `balance aGeRSnWykicBEGESPbTXPQ0_q2IiMLBBMyemu2pBYoA` //Defaults to AO Balance
- `balance aGeRSnWykicBEGESPbTXPQ0_q2IiMLBBMyemu2pBYoA KmGmJieqSRJpbW6JJUFQrH3sQPEG9F6DQETlXNt4GpM` //FIRE token balance
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


