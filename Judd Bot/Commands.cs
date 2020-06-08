using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Judd_Bot
{
    public class Commands : BaseCommandModule
    {
        [Command("wood")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync(
                "\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⡀⠄⠄⠄⢀⠄⠄⠈⠄⠄⠄⠄⠄⠄⠄⠠⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣌⠄⠪⠢⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⡐⠜⠄⠄⠄⡠⣄⠠⠄⠄⠄⠄⠄⠄⠄⠄⣘⢢⣤⡀⠄⠄⠙⠒⠒⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠤⠯⡂⠁⢠⣴⣿⣿⣿⣦⠄⢀⠄⠄⠄⢠⣴⣔⣿⣿⣿⣿⣷⡀⠄⠄⠄⢰⠉⣁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⣀⣤⡾⠂⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣷⣺⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡀⠄⠄⠈⠄⠂⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⣠⣒⠑⠄⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠄⠄⠄⠐⡁⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⢼⡇⠄⢰⢺⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆⠄⠄⣹⡗⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠠⣿⠇⠄⢂⡜⠛⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠄⠐⡃⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⢀⢛⠄⠄⠈⡿⣷⣶⡌⠉⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⢟⣫⣿⣷⣶⡯⣿⡆⠄⠠⡀⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠘⠄⠄⠄⠄⢸⡜⣿⣿⣷⣄⡀⠙⠿⣿⣿⣿⣿⣿⡿⠟⢁⣠⣾⠿⣿⣿⣿⣿⣾⡿⠠⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠐⠄⠄⠄⠄⠘⣡⣿⣿⠃⠉⠛⢦⣀⠈⠛⣿⣿⠋⠄⡤⠞⠋⠄⢠⣿⣿⡿⣿⣿⡇⠄⠄⢠⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠠⠶⢄⠄⠄⣾⡰⠟⠉⠁⠄⠄⠄⠄⠈⠁⠄⢹⡇⠄⠈⠄⠄⠄⠄⠄⠉⠛⢷⣹⣿⡇⠄⢂⡴⡦⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠠⢀⠄⠈⡷⠅⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣸⣧⡀⠄⠄⠄⠄⠄⠄⠄⣀⣘⣿⣿⡏⠄⢈⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠈⠄⠄⣿⣷⣷⣲⣤⣀⣀⣀⣤⣦⣦⣾⣿⣿⣿⣶⣴⣆⣤⣤⣴⣶⣿⣿⣿⣿⣗⡄⢙⡇⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⣀⣀⡇⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣾⠇⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠘⠛⢀⠄⠙⠛⠛⠛⠛⠉⠭⠚⠉⠛⠛⠛⠛⠛⠛⣽⣷⣤⠉⠛⣿⣿⣿⣿⣿⣿⣿⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠈⠛⠄⠄⠄⣤⡆⠄⠄⠈⣽⣤⣀⣀⠄⣀⣠⣾⣿⣿⣿⣆⠄⢸⣿⣿⣿⣿⡿⠟⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢸⣧⡄⠄⠄⠄⣤⣤⣤⣭⣭⣤⣶⣶⣦⡌⢿⠗⣼⣿⣿⣿⣿⡇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢸⣿⣇⠄⠄⡴⠉⠉⠉⡉⣉⣩⣿⣿⣿⣿⠄⠄⠘⣿⡟⠋⠘⠃⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⡀⡀⠄⢻⣿⠄⢸⣏⣿⣿⣷⣿⣿⣿⣿⣿⣿⣿⡀⢀⣰⣿⡇⠄⣠⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⠄⠄⢠⣴⣶⣿⣿⣿⣿⡀⠘⠻⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣿⣾⣿⡿⠃⢀⣿⣧⣀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⢀⣼⣿⣿⣿⣿⣿⣿⣿⣷⠄⠄⠄⠄⠄⠄⠈⠉⠉⠉⠄⠄⠄⠄⠄⠙⠛⠋⣀⣸⣿⣿⣿⠏⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⣤⣶⣿⣿⣿⡿⠃⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⣶⣶⣿⣿⣿⣿⣿⠟⠄⠄⠄⠄⠄⠄⠁⠔⣄⡀⠄⠄⠄⠄\n⠄⠄⠄⠄⠄⣠⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣄⡀⠄⠄⠄⠄⢀⣠⣴⣿⣿⣿⣿⣿⣿⣿⡿⠇⠄⠄⠄⠄⠄⠈⠄⠈⠉⠩⠉⢙⠷⡄⣀\n");
        }

        [Command("echo")]
        public async Task Echo(CommandContext ctx, params string[] message)
        {
            var tosendmessage = "";
            foreach (var elem in message) tosendmessage = tosendmessage + elem + " ";
            await ctx.RespondAsync(tosendmessage);
        }

        [Command("math")]
        [Description("Does simple math, (+, -, *, /), called with \"!math (first num.) (operation) (second num.)\"")]
        [Aliases("maths")]
        public async Task Add(CommandContext ctx, float one, string type, float two)
        {
            var upper = type.ToUpper();
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
        public async Task Assign(CommandContext ctx, string message)
        {
            var intakeStrings = message.Split(' ').ToList();
            var classes = "";
            var id = intakeStrings[0];
            intakeStrings.RemoveAt(0);
            foreach (var elem in intakeStrings) classes = classes + elem + " ";

            var rolestoadd = classes.Split(',');
            var token = File.ReadAllText(@"token.txt");

            var discord = new DiscordRestClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            var userid = Convert.ToUInt64(id);
            var guild = await discord.GetGuildAsync(718945666348351570);
            foreach (var elem in rolestoadd)
            {
                var trimmed = elem.Trim();
                if (guild.Roles.Any(tr => tr.Value.Name.Equals(trimmed)))
                {
                    var roleid = guild.Roles.FirstOrDefault(x => x.Value.Name.ToString() == trimmed).Key;
                    await discord.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
                }
                else
                {
                    var role = await guild.CreateRoleAsync(trimmed, Permissions.SendMessages);
                    var channel = await guild.CreateChannelAsync(trimmed, ChannelType.Text);
                    var voicechannel = await guild.CreateChannelAsync(trimmed, ChannelType.Voice);
                    await discord.ModifyChannelAsync(channel.Id, channel.Name, 0, "", false, 718991556107042817,
                        null, 0, 0, "");
                    await discord.ModifyChannelAsync(voicechannel.Id, voicechannel.Name, 0, "", false,
                        718945666797404230,
                        64000, 0, 0, "");
                    await channel.AddOverwriteAsync(role, Permissions.AccessChannels);
                    await voicechannel.AddOverwriteAsync(role, Permissions.AccessChannels);
                    await channel.AddOverwriteAsync(guild.EveryoneRole, Permissions.None, Permissions.AccessChannels);
                    await voicechannel.AddOverwriteAsync(guild.EveryoneRole, Permissions.None,
                        Permissions.AccessChannels);
                    await discord.AddGuildMemberRoleAsync(718945666348351570, userid, role.Id, "");
                }
            }
        }
    }
}