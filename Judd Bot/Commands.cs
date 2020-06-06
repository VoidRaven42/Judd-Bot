using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace Judd_Bot
{
    public class Commands : BaseCommandModule
    {
        [Command("getid")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync(ctx.Guild.Id.ToString());
        }

        [Command("math")]
        [Description("Does simple math, (+, -, *, /), called with \"!math (first num.) (operation) (second num.)\"")]
        [Aliases("maths")]
        public async Task Add(CommandContext ctx, float one, string type, float two)
        {
            string upper = type.ToUpper();
            switch (upper)
            {
                case "+":
                    await ctx.RespondAsync($"Result: {one + two}");
                    Console.WriteLine($"[COMMAND] ({ctx.User.Username}) Add {one} and {two}, result {one + two}");
                    break;
                case "-":
                    await ctx.RespondAsync($"Result: {one - two}");
                    Console.WriteLine($"[COMMAND] ({ctx.User.Username}) Subtract {two} from {one}, result {one - two}");
                    break;
                case "*":
                    await ctx.RespondAsync($"Result: {one * two}");
                    Console.WriteLine($"[COMMAND] ({ctx.User.Username}) Times {one} and {two}, result {one * two}");
                    break;
                case "x":
                    await ctx.RespondAsync($"Result: {one * two}");
                    Console.WriteLine($"[COMMAND] ({ctx.User.Username}) Times {one} and {two}, result {one * two}");
                    break;
                case "/":
                    await ctx.RespondAsync($"Result: {one / two}");
                    Console.WriteLine($"[COMMAND] ({ctx.User.Username}) Divide {one} by {two}, result {one / two}");
                    break;
            }

            
        }
    }

    [Group("admin")]
    [Hidden]
    [RequirePermissions(Permissions.ManageGuild)]
    public class AdminCommands : BaseCommandModule
    {
        private static DiscordRestClient discord;
        private static string token = File.ReadAllText(@"token.txt");

        [Command("assignroles")]
        public async Task Assign(string id, string classrooms)
        {
            discord = new DiscordRestClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            discord.AddGuildMemberRoleAsync()
        }
    }
}