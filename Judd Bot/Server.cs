using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using MySql.Data.MySqlClient;
using static System.Console;

namespace Judd_Bot
{
    internal class Server
    {
        private static readonly string dbinfo = File.ReadAllText(@"sqlinfo.txt");
        private static readonly string token = File.ReadAllText(@"token.txt");

        public readonly MySqlConnection conn = new MySqlConnection(dbinfo);

        public readonly DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {Token = token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug});

        private readonly DiscordRestClient discordrest = new DiscordRestClient(new DiscordConfiguration
            {Token = token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug});


        public Server()
        {
            try
            {
                conn.Open();
                WriteLine("Connected to SQL DB");
            }
            catch (Exception e)
            {
                WriteLine(e);
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

                if (email.EndsWith("@judd.kent.sch.uk"))
                {
                }
                else
                {
                    await KickUser(usertofind);
                    sql = $"DELETE FROM users WHERE d_user_snowflake='{usertofind}'";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    return;
                }

                sql = $"SELECT g_class_id FROM enrollments WHERE email='{email}'";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read()) classroomids = classroomids + rdr[0] + ',';

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
                for (var i = 0; i < rolelist.Count; i++)
                    if (rolelist[i] == "")
                        await AssignSingleRole(usertofind, classlist[i], idlist[i]);
                    else
                        await AssignExistingRole(usertofind, rolelist[i]);
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
                return;
            await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
        }

        public async Task KickUser(string id)
        {
            var guild = await discord.GetGuildAsync(718945666348351570);
            var member = await guild.GetMemberAsync(Convert.ToUInt64(id));
            await discordrest.RemoveGuildMemberAsync(718945666348351570, Convert.ToUInt64(id), "Not Judd Email");
            await member.SendMessageAsync(
                "Hi! Judd Bot here!\nYou tried to authenticate with an email not from Judd. Please go back to the website and log in with your school Google Account.");
        }

        public async Task AssignSingleRole(string id, string roletoadd, string classid)
        {
            var userid = Convert.ToUInt64(id);
            var guild = await discordrest.GetGuildAsync(718945666348351570);
            var trimmed = roletoadd.Trim();
            if (guild.Roles.Any(tr => tr.Value.Name.Equals(trimmed)))
            {
                var roleid = guild.Roles.FirstOrDefault(x => x.Value.Name.ToString() == trimmed).Key;
                await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, roleid, "");
                var sql = $"UPDATE classes SET d_role_snowflake='{roleid}' WHERE g_class_id='{classid}'";
                var cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            else
            {
                var role = await guild.CreateRoleAsync(trimmed, Permissions.SendMessages);
                var channel = await guild.CreateChannelAsync(trimmed, ChannelType.Text);
                await channel.AddOverwriteAsync(role, Permissions.AccessChannels);
                await channel.AddOverwriteAsync(guild.EveryoneRole, Permissions.None, Permissions.AccessChannels);
                await discordrest.AddGuildMemberRoleAsync(718945666348351570, userid, role.Id, "");
                var sql = $"UPDATE classes SET d_role_snowflake='{role.Id}',d_text_channel_snowflake='{channel.Id}' WHERE g_class_id='{classid}'";
                var cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}