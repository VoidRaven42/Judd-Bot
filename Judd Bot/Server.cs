using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                Console.WriteLine("Connecting to Database");
                conn.Open();
                Console.WriteLine("Connected");
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
                Console.WriteLine(email);
                rdr.Close();

                sql = $"SELECT g_class_id FROM enrollments WHERE email='{email}'";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0]);
                    classroomids = classroomids + rdr[0] + ',';
                }
                rdr.Close();
                //Console.WriteLine(classroomids);

                var splitids = classroomids.Split(',');
                var idlist = splitids.ToList();
                foreach (var elem in idlist)
                {
                    sql = $"SELECT class_display_name, d_role_snowflake FROM classes WHERE g_class_id='{elem}'";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr[0]);
                        classroomnames = classroomnames + rdr[0] + ',';
                        rolestocheck = rolestocheck + rdr[1] + ',';
                    }
                    rdr.Close();
                }
                var classlist = classroomnames.Split(',').ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("SQL query complete.");
        }
    }
}