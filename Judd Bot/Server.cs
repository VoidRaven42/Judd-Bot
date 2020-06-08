using System;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Judd_Bot
{
    internal class Server
    {
        public static async Task SQLQuery(string usertofind)
        {
            var dbinfo = File.ReadAllText(@"sqlinfo.txt");
            var conn = new MySqlConnection(dbinfo);
            try
            {
                Console.WriteLine("Connecting to Database...");
                conn.Open();

                var sql = "SELECT Name, HeadOfState FROM Country WHERE Continent='Oceania'"; //Placeholder
                var cmd = new MySqlCommand(sql, conn);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read()) Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("SQL query complete.");
        }
    }
}