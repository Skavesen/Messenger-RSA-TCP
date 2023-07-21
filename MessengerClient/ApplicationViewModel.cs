using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using System.ComponentModel;
using RSAEncryption;
using ClientsInfo;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Threading;


namespace MessengerClient
{
    //Паттерн MVVM
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        //Конструктор для установления IP и порта для прослушивания
        public ApplicationViewModel(IPAddress host, int port)
        {
            //Возвращает данный объект в виде строки
            Host = host.ToString();
            Port = port;
            //Выделение памяти под сообщения
            Messages = new ObservableCollection<MessageContent>();
            //Связь команд и методов
            SendMessageCommand = new DelegateCommand(SendMessage);
            ConnectCommand = new DelegateCommand(Connect);
            DisconnectCommand = new DelegateCommand(Disconnect);
        }
        #region objects
        RSA rSA = new RSA();
        TcpClient tcp;
        NetworkStream network;
        ClientsConnected clients;
        //Коллекция, которая оповещает внешние объекты об изменении
        public ObservableCollection<MessageContent> Messages { get; set; }
        public string Host { get; set; }
        //Имя клиента при подлкючении
        public string Login { get; set; } = AuthWindow.username;
        public int Port { get; set; }
        #endregion

        #region Keys
        //Ключи клиента
        public long ClientE => rSA.e;
        public long ClientN => rSA.n;
        //Ключи собеседника в виде свойств
        public long FriendE { get => friendE; set { friendE = value; OnPropertyChanged(); } }
        public long FriendN { get => friendN; set { friendN = value; OnPropertyChanged(); } }
        //Ключи собеседника
        long friendE;
        long friendN;
        #endregion

        #region Text
        //Текст который вводит клиент
        public string Text
        {
            get => _text; set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
        private string _text;
        #endregion

        #region bool
        //Условия для активации кнопки подлкючения
        public bool Connected => FriendE != 0 && FriendN != 0 && CanConnect;
        //Переменная для определения возможности подлкючения
        bool CanConnect = true;
        //Переменная для определения возможности отключения
        public bool CanDisconnect { get; set; } = false;
        #endregion

        #region Command
        //Команда для подключения к серверу
        public ICommand ConnectCommand { get;set; }
        //Метод подлкючения
        private void Connect()
        {
            //Создание нового клиента
            tcp = new TcpClient();

            try
            {
                //Попытка подлкючения к серверу
                tcp.Connect(Host, Port);
            }
            //Ошибки
            catch (Exception e)
            {
                //Вывод ошибки
                MessageBox.Show(e.Message,"Cервер отключен!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Сетевой поток для чтения и записи
            network = tcp.GetStream();
            //Объявленире клиента подключенным
            clients = new ClientsConnected(Login);
            //Запуск вложенных задач
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //Объект для сериализации и десериализации объектов
                    BinaryFormatter bf = new BinaryFormatter();
                    //Сериализация потока чтения/записи сервера и клиентов
                    bf.Serialize(network, clients);
                    //Пока клиент подлючен
                    while (tcp.Connected)
                    {
                        //Десериализация
                        object obj = bf.Deserialize(network);
                        //Проверка является ли объект подключенным клиентом
                        if (obj is ClientsConnected conClient)
                        {
                            //Изменение возможностей подлкючения и отключения
                            CanConnect = false;
                            CanDisconnect = true;
                            //Рассылает уведомление синхронно в потоке
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //Сообщение о подлкючении
                                Messages.Add(new MessageContent() { Text = $"Новое подключение: {conClient.Login}", Time = DateTime.Now });
                            });
                            //Обновление свойств
                            OnPropertyChanged();
                        }
                        //Проверяет объект является ли он сообщением
                        else if (obj is MessageSending msg)
                        {
                            //Пропускает расшифрофку сообщения для самого клиента
                            if (msg.clientsConnected.Login == clients.Login)
                                continue;
                            //Расшифровка соообщения в потоке
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //Расшифровка
                                string text = rSA.Decrypt(msg.messages.text);
                                //Рассылка сообщения
                                Messages.Add(msg.messageContent(text));
                            }, DispatcherPriority.Background);
                        }
                    }
                }
                catch (Exception e)
                {
                    //Сообщение об ошибке
                    Console.WriteLine(e.Message);
                    //Изменение возможностей подлкючения и отключения
                    CanConnect = true;
                    CanDisconnect = false;
                    //Обновление свойств
                    OnPropertyChanged();
                }
            });
        }
        //Команда для отключения от сервера
        public ICommand DisconnectCommand { get; set; }
        //Метод отключения
        private void Disconnect()
        {
            //Удаление клиента
            tcp?.Close();
            //Изменение возможностей подлкючения и отключения
            CanConnect = true;
            CanDisconnect = false;
            //Сообщение об отключении
             Messages.Add(new MessageContent() { Text = "Отключение от сервера", Time = DateTime.Now });
            //Обновление свойств
            OnPropertyChanged();

        }
        //Команда отправки сообщения
        public ICommand SendMessageCommand { get; set; }
        //Метод отправки сообщения
        private void SendMessage()
        {
            try
            {
                //Шифрование сообщения с помощью ключа собеседника
                string [] text = rSA.Encrypt(Text, FriendE, FriendN);
                //Отправка сообщения самому себе
                Messages.Add(new MessageContent() { Login = clients.Login, Text = Text, Time = DateTime.Now });
                //Отправка зашифрованного сообщения на консоль
                MessageSending message = new MessageSending(new Messages(text, DateTime.Now), clients);
                new BinaryFormatter().Serialize(network, message);
                //Обновляет строку сообщения
                Text = string.Empty;
                OnPropertyChanged(nameof(Text));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка отправки сообщения", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
        #endregion

        //Метод для обновления свойст(паттерн MVVM)
        public void OnPropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
       
    }
}
