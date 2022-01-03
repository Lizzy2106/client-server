using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ClientServer
{
    class Program
    {
        /*static void Main(string[] args)
        {
            SocketConfig server = new SocketConfig();

            server.Address = "10.169.128.183";
            server.NbrToListen = 1;
            server.Port = 9050;

            Socket listener = server.Connect();
            Socket client =  server.AllowConnection(listener);
            System.Threading.Thread.Sleep(100);

            *//*while (true)
            {
                server.SendMessage(client, "tesh");
            }*//*

            while (true)
            {

                object message = server.ReceiveMessage(client);

                try
                {
                    Console.WriteLine((string)message);
                    //string output = JsonConvert.SerializeObject((string)message);
                    // Console.WriteLine(output);
                    //JsonConvert.Deserialize(json, typeof(object));
                    MessageModule config = JsonConvert.DeserializeObject<MessageModule>((string)message);
                    Console.WriteLine(config.formatlog);
                }
                catch (Exception e)
                {
                    Thread.Sleep(100);
                    Console.WriteLine(e.Message);
                }

            }
        }*/

        static void Main(string[] args)
        {
            String msg = "{\"lang\": \"fr\", \"formatlog\": \"json\"}";
            Console.WriteLine(msg);
            MessageModule config = JsonConvert.DeserializeObject<MessageModule>(msg);
            Console.WriteLine(config.formatlog);
        }
    }

    public class MessageModule
    {
        public string lang { get; set; }
        public string formatlog { get; set; }
    }
}
