using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"Hi, {ctx.User.Username}!");
        }

        [Command("echo")]
        public async Task Echo(CommandContext ctx, params string[] message)
        {
            string tosendmessage = "";
            foreach (var elem in message)
            {
                tosendmessage = tosendmessage + elem + " ";
            }
            await ctx.RespondAsync(tosendmessage);
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

    [RequirePermissions(Permissions.ManageGuild)]
    public class AdministrationCommands : BaseCommandModule
    {
        [Command("getid")]
        public async Task Getid(CommandContext ctx)
        {
            await ctx.RespondAsync(ctx.Guild.Id.ToString());
        }

        [Command("assignroles")]
        public async Task Assign(string id, [RemainingText]String remaining)
        {
            var intakeStrings = remaining.Split(' ').ToList();
            string classes = "";
            foreach (var elem in intakeStrings)
            {
                classes = classes + elem + " ";
            }

            var rolestoadd = classes.Split(',');
            string token = File.ReadAllText(@"token.txt");

            DiscordRestClient discord = new DiscordRestClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            var userid = Convert.ToUInt64(id);
            DiscordGuild guild = await discord.GetGuildAsync(718945666348351570);
            foreach (var elem in rolestoadd)
            {
                if (guild.Roles.Any(tr => tr.Value.Name.Equals(elem)))
                {
                    var roleid = guild.Roles.FirstOrDefault(x => x.Value.Name.ToString() == elem).Key;
                    await discord.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
                }
                else
                {
                    var name = elem;
                    var role = await guild.CreateRoleAsync(name, permissions:Permissions.SendMessages);
                    var channel = await guild.CreateChannelAsync(elem, ChannelType.Text);
                    await channel.AddOverwriteAsync(role, Permissions.SendMessages);
                    await discord.AddGuildMemberRoleAsync(718945666348351570, userid, role.Id, "");
                    await discord.ModifyChannelAsync(channel.Id, channel.Name, 0, "", false, parent: 718991556107042817,
                        bitrate: null, userLimit: 0, perUserRateLimit: 0, "");
                }
            }
        }
    }
}