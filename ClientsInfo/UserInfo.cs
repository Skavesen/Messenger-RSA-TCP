using System;
using System.Net.Sockets;

namespace ClientsInfo
{
    [Serializable]
    public class UserInfo
    {
        //Уникальный идетнификатор
        public Guid ID { get; set; }
        //Имя клиента
        public string Login { get; set; }
    }
    [Serializable]
    //Подключенные клиенты
    public class ClientsConnected : UserInfo
    {
        public ClientsConnected(string login)
        {
            Login = login;
        }
        
    }
    public class SClients : UserInfo
    {
        //Конструктор по умолчанию
        public SClients()
        {
                
        }
        //У каждого клиента свой логин(имя), и свой уникальный идетнивикатор(id)
        public SClients(TcpClient tcpClient,Guid id,string login)
        {
            TcpClient = tcpClient;
            Login = login;
            ID = id;
        }
        //Объект для клиентского подлкючения к сети
        public TcpClient TcpClient;
    }
}
