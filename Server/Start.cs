using System;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class Start
    {
        static void Main(string[] args)
        {
            //Запуск сервера
            Server server = new Server(SearchIP(),5050);
            server.Start();
        }
        //определение ip
        public static IPAddress SearchIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;

            throw new Exception("Ошибка");
        }
    }
}
