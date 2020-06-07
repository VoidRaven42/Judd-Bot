using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;


namespace Judd_Bot
{
    public class Program
    {
        private static DiscordClient discord;
        private static CommandsNextExtension commands;
        private static string token = File.ReadAllText(@"token.txt");

        public static void Main(string[] args)
        {
            try
            {
                MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            string[] prefixes = { "!" };
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = prefixes
            });

            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<AdministrationCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}