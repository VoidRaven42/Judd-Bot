using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using static System.Console;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;

namespace Judd_Bot
{
    internal class Server
    {
        private static readonly string dbinfo = File.ReadAllText(@"sqlinfo.txt");
        private static string token = File.ReadAllText(@"token.txt");

        private MySqlConnection conn = new MySqlConnection(dbinfo);

        private DiscordRestClient discordrest = new DiscordRestClient(new DiscordConfiguration {Token = token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug});
        private DiscordClient discord = new DiscordClient(new DiscordConfiguration {Token = token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug});


        public Server()
        {
            try
            {
                conn.Open();
                WriteLine("Connected to SQL DB");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SQLQuery(string usertofind)
        {
            try
            {
                var sql = $"SELECT email FROM users WHERE d_user_snowflake='{usertofind}'";
                var cmd = new MySqlCommand(sql, conn);
                var rdr = cmd.ExecuteReader();
                var email = "";
                var classroomids = "";
                var classroomnames = "";
                var rolestocheck = "";

                while (rdr.Read()) email = email + rdr[0];
                rdr.Close();

                sql = $"SELECT g_class_id FROM enrollments WHERE email='{email}'";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    classroomids = classroomids + rdr[0] + ',';
                }
                rdr.Close();

                var splitids = classroomids.Split(',');
                var idlist = splitids.ToList();
                idlist.RemoveAt(idlist.Count - 1);
                foreach (var elem in idlist)
                {
                    sql = $"SELECT class_display_name, d_role_snowflake FROM classes WHERE g_class_id='{elem}'";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        classroomnames = classroomnames + rdr[0] + ',';
                        rolestocheck = rolestocheck + rdr[1] + ',';
                    }
                    rdr.Close();
                }
                var classlist = classroomnames.Split(',').ToList();
                classlist.RemoveAt(classlist.Count - 1);
                var rolelist = rolestocheck.Split(',').ToList();
                rolelist.RemoveAt(rolelist.Count - 1);
                for (int i = 0; i < rolelist.Count; i++)
                {
                    if (rolelist[i] == "")
                    {
                        var classrole = await AssignSingleRole(usertofind, classlist[i]);
                        sql = $"UPDATE classes SET d_role_snowflake='{classrole}' WHERE g_class_id='{idlist[i]}'";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        await AssignExistingRole(usertofind, rolelist[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
            }

            WriteLine("SQL operation complete.");
        }

        public async Task AssignExistingRole(string id, string roletoadd)
        {
            var roleid = ulong.Parse(roletoadd);

            var userid = Convert.ToUInt64(id);
            var guild = await discord.GetGuildAsync(718945666348351570);
            var user = await guild.GetMemberAsync(userid);
            if (user.Roles.Any(tr => tr.Name.Equals(roletoadd)))
            {
                return;
            }
            else
            {
                await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
            }
        }

        public async Task<string> AssignSingleRole(string id, string roletoadd)
        {
            
            var userid = Convert.ToUInt64(id);
            var guild = await discordrest.GetGuildAsync(718945666348351570);
            var trimmed = roletoadd.Trim();
            if (guild.Roles.Any(tr => tr.Value.Name.Equals(trimmed)))
            {
                var roleid = guild.Roles.FirstOrDefault(x => x.Value.Name.ToString() == trimmed).Key;
                await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
                return roleid.ToString();
            }
            else
            {
                var role = await guild.CreateRoleAsync(trimmed, Permissions.SendMessages);
                var channel = await guild.CreateChannelAsync(trimmed, ChannelType.Text);
                var voicechannel = await guild.CreateChannelAsync(trimmed, ChannelType.Voice);
                await discordrest.ModifyChannelAsync(channel.Id, channel.Name, 0, "", false, 718991556107042817,
                    null, 0, 0, "");
                await discordrest.ModifyChannelAsync(voicechannel.Id, voicechannel.Name, 0, "", false,
                    718945666797404230,
                    64000, 0, 0, "");
                await channel.AddOverwriteAsync(role, Permissions.AccessChannels);
                await voicechannel.AddOverwriteAsync(role, Permissions.AccessChannels);
                await channel.AddOverwriteAsync(guild.EveryoneRole, Permissions.None, Permissions.AccessChannels);
                await voicechannel.AddOverwriteAsync(guild.EveryoneRole, Permissions.None,
                    Permissions.AccessChannels);
                await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, role.Id, "");
                return role.Id.ToString();
            }
            
        }
    }
}