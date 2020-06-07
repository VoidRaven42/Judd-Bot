using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Judd_Bot;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Judd_Bot
{
    class Server
    {
        public static async Task SQLQuery(string usertofind)
        {
            var dbinfo = File.ReadAllText(@"sqlinfo.txt");
            MySqlConnection conn = new MySqlConnection(dbinfo);
            try
            {
                Console.WriteLine("Connecting to Database...");
                conn.Open();

                string sql = "SELECT Name, HeadOfState FROM Country WHERE Continent='Oceania'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
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

    