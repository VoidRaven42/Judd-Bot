using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using static System.Console;
using System.Threading.Tasks;
using DSharpPlus;
using MySql.Data.MySqlClient;

namespace Judd_Bot
{
    internal class Server
    {
        private static readonly string dbinfo = File.ReadAllText(@"sqlinfo.txt");
        private MySqlConnection conn = new MySqlConnection(dbinfo);

        public Server()
        {
            try
            {
                conn.Open();
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
                var rolelist = rolestocheck.Split(',').ToList();
                for (int i = 0; i < rolelist.Count; i++)
                {
                    if (rolelist[i] == "")
                    {
                        var classrole = await AssignSingleRole(usertofind, classlist[i]);
                    }
                    else
                    {
                        AssignExistingRole(usertofind, rolelist[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
            }

            WriteLine("SQL query complete.");
        }

        public async Task AssignExistingRole(string userid, string roleid)
        {
            
        }

        public async Task<string> AssignSingleRole(string id, string roletoadd)
        {
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
            
            var trimmed = roletoadd.Trim();
            if (guild.Roles.Any(tr => tr.Value.Name.Equals(trimmed)))
            {
                var roleid = guild.Roles.FirstOrDefault(x => x.Value.Name.ToString() == trimmed).Key;
                await discord.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
                return roleid.ToString();
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
                return role.Id.ToString();
            }
            
        }
    }
}