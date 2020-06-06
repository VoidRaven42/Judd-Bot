using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using Judd_Bot;

namespace Judd_Bot
{
    class Server
    {
        TcpListener server = null;
        public Server(int port)
        {
            IPAddress localAddr = IPAddress.Any;
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener(localAddr.ToString());
        }
        public void StartListener(string ip)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\nWaiting for a connection\n");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }
        public void HandleDevice(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            string imei = String.Empty;
            string data = null;
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);
                    if (data.StartsWith("[DATA]"))
                    {
                        string str = "Data Recieved: ";
                        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str + data.Remove(0, 7) + "\n");
                        stream.Write(reply, 0, reply.Length);
                        Console.WriteLine("{1}: Response Returned: {0}", str + data.Remove(0, 7), Thread.CurrentThread.ManagedThreadId);
                        var todo = data.Remove(0, 7);
                        var splitted = todo.Split(new[] { ',' }, 2);
                        foreach (var elem in splitted)
                        {
                            Console.WriteLine(elem);
                        }
                        //AdminCommands admin = new AdminCommands();
                        //assign.Assign(splitted[0], splitted[1]);
                    }
                    else
                    {
                        string str = "Invalid Request\n";
                        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                        stream.Write(reply, 0, reply.Length);
                        Console.WriteLine("{1}: Response Returned: {0}", str, Thread.CurrentThread.ManagedThreadId);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }
    }
}

