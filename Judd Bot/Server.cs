using System;
using System.IO;
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

                while (rdr.Read()) Console.WriteLine(rdr[0]);
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("SQL query complete.");
        }
    }
}