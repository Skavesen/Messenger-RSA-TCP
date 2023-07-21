using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using ClientsInfo;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Net;

namespace Server
{
    public class Server
    {
        //Объект для прослушивание подключений от TCP-клиентов
        static TcpListener tcpListener;
        //Список клиентов сервера
        static List<SClients> sClients;

        public Server(IPAddress host, int port)
        {
            //Ожидание входящих попыток подключения, с заданным ip и port
            tcpListener = new TcpListener(host, 5050);
            //Выделение памяти для списка клиентов
            sClients = new List<SClients>();
            //Вывод сообщения о запуске сервера на консоль
            Console.WriteLine("Сервер создан!");
            Console.WriteLine("Host:{0}  Port:{1}", host, port);
        }
        //Метод старта сервера
        public void Start()
        {
            //Запуск сервера
            tcpListener.Start();
            //Вывод сообщения на консоль
            Console.WriteLine("Начало работы сервера!");
            //Ожидание подключений в бесконечном цикле
            while (true)
            {
                //Получение объекта для взаимодействия с подключенными клиентами
                TcpClient client = tcpListener.AcceptTcpClient();
                //Запуск различных задач, выполняющихся независимо друг от друга
                Task.Factory.StartNew(() =>
                {
                    //Сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();
                    //Объект для сериализации и десериализации объектов
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    //Объект класса с содержанием данных клиента
                    SClients SC = new SClients();
                    //Десериализация потока данных подлкюченных клиентов
                    ClientsConnected conClient = binaryFormatter.Deserialize(stream) as ClientsConnected;
                    //Передача данных подлкюченного клиента
                    SC = new SClients(client, conClient.ID, conClient.Login);
                    //Добавление клиента в список подключенных клиентов
                    sClients.Add(SC);
                    //Вывод сообщения о подключении на консоль
                    ConnectionInformation(conClient);
                    //Рассылка сообщения клиентам
                    MessageToClients(conClient);
                    //Цикл для подключенных клиентов
                    while (client.Client.Connected)
                    {
                        try
                        {
                            //Десериализация потока данных
                            MessageSending messageSending = binaryFormatter.Deserialize(stream) as MessageSending;
                            //Вывод сообщения клиента на консоль(зашифрованное)
                            Console.WriteLine($"[{messageSending.clientsConnected.Login}] " + $"{string.Join("", messageSending.messages.text)}");
                            //Рассылка сообщения клиентам
                            MessageToClients(messageSending);
                        }
                        //Вывод ошибок
                        catch (Exception e)
                        {
                            //Определение состояния клиента
                            if (client.Client.Poll(0, SelectMode.SelectRead))
                            {
                                //Проверка подключен ли клиент
                                if (client.Client.Connected == false)
                                {
                                    //Отключение клиента(true-сокет может быть повторно использовандля подключения)
                                    client.Client.Disconnect(true);
                                }
                            }
                            //Изменение цвета текста в консоли
                            Console.ForegroundColor = ConsoleColor.Red;
                            //Вывод ошибки на консоль
                            Console.WriteLine(e.Message);
                            //Изменение цвета текста в консоли
                            Console.ForegroundColor = ConsoleColor.White;
                            //Завершение потока
                            Thread.CurrentThread.Abort();
                        }
                    }
                });
            }

        }
        //Метод рассылки сообщения клиентам
        private static void MessageToClients(object msg)
        {
            //Объект для сериализации и десериализации объектов
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //Перебор всех клиентов
            foreach(SClients client in sClients.ToArray())
            {
                if (client.TcpClient.Connected)
                {
                    try
                    {
                        //Сериализация
                        binaryFormatter.Serialize(client.TcpClient.GetStream(), msg);
                    }
                    catch
                    {
                    }
                }
            }
        }
       
        //Отправка сообщения о подлкючении клиента
        private static void ConnectionInformation (ClientsConnected clientsConnected)
        {
            //Изменение цвета текста в консоли
            Console.ForegroundColor = ConsoleColor.Green;
            //Вывод информации о подключении на консоль
            Console.WriteLine($"Новое подключение: {clientsConnected.Login}");
            //Изменение цвета текста в консоли
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
