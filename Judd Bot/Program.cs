using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace Judd_Bot
{
    public class Program
    {
        private static DiscordClient discord;
        private static CommandsNextExtension commands;
        private static readonly string token = File.ReadAllText(@"token.txt");

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

            discord.GuildMemberAdded += Discord_GuildMemberAdded;

            string[] prefixes = {"!"};
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = prefixes
            });

            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<AdministrationCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task Discord_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            await Server.SQLQuery(e.Member.Id.ToString());
        }
    }
}