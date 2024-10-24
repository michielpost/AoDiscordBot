using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AoDiscordBot.Services
{
    public class DiscordBotBackgroundService : BackgroundService
    {
        private readonly ILogger<DiscordBotBackgroundService> _logger;
        private readonly DiscordSocketClient discordSocketClient;
        private readonly CommandHandlingService commandHandlingService;
        private readonly IConfiguration _configuration;

        public DiscordBotBackgroundService(
            ILogger<DiscordBotBackgroundService> logger,
            DiscordSocketClient discordSocketClient,
            CommandHandlingService commandHandlingService,
            IConfiguration configuration)
        {
            _logger = logger;
            this.discordSocketClient = discordSocketClient;
            this.commandHandlingService = commandHandlingService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("MyBackgroundService is running at: {time}", DateTimeOffset.Now);

                // Your async task goes here
                await YourAsyncTask();

                // Wait for a certain period before running again
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task YourAsyncTask()
        {
            //_client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = _configuration["DiscordBot:Token"];

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Discord bot token is missing in the configuration.");
                return;
            }

            //_client.MessageReceived += MessageReceivedAsync;
            //_client.InteractionCreated += InteractionCreatedAsync;

            await discordSocketClient.LoginAsync(TokenType.Bot, token);
            await discordSocketClient.StartAsync();

            // Here we initialize the logic required to register our commands.
            await commandHandlingService.InitializeAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        // This is not the recommended way to write a bot - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == discordSocketClient.CurrentUser.Id)
                return;


            if (message.Content == "!ping")
            {
                // Create a new ComponentBuilder, in which dropdowns & buttons can be created.
                var cb = new ComponentBuilder()
                    .WithButton("Click me!", "unique-id", ButtonStyle.Primary);

                // Send a message with content 'pong', including a button.
                // This button needs to be build by calling .Build() before being passed into the call.
                await message.Channel.SendMessageAsync("pong!", components: cb.Build());
            }
        }

        // For better functionality & a more developer-friendly approach to handling any kind of interaction, refer to:
        // https://discordnet.dev/guides/int_framework/intro.html
        private static async Task InteractionCreatedAsync(SocketInteraction interaction)
        {
            // safety-casting is the best way to prevent something being cast from being null.
            // If this check does not pass, it could not be cast to said type.
            if (interaction is SocketMessageComponent component)
            {
                // Check for the ID created in the button mentioned above.
                if (component.Data.CustomId == "unique-id")
                    await interaction.RespondAsync("Thank you for clicking my button!");

                else
                    Console.WriteLine("An ID has been received that has no handler!");
            }
        }
    }
}
