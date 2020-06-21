using System;
using System.IO;
using System.Threading;
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
        private static Server server = new Server();

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
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

            commands.CommandErrored += Commands_CommandErrored;

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            await e.Context.RespondAsync("Incorrect usage of command\nCommand Help:");
            var help = new CommandsNextExtension.DefaultHelpModule();
            await help.DefaultHelpAsync(e.Context, e.Command.Name);
        }

        private static async Task Discord_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            await server.SQLQuery(e.Member.Id.ToString());
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnecting, then exiting in 5 seconds");
            discord.DisconnectAsync();
            Thread.Sleep(5000);
        }
    }
}