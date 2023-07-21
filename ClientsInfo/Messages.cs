using System;

namespace ClientsInfo
{
    [Serializable]
    //Класс для обмена сообщениями
   public class MessageSending
    {
        //Чтение подключенных клиентов
        public ClientsConnected clientsConnected { get; }
        //Чтение сообщения
        public Messages messages { get; }
        public MessageSending(Messages messages,ClientsConnected clientsConnected)
        {
            this.clientsConnected = clientsConnected;
            this.messages = messages;
        }
        
       
    }
    [Serializable]
    //Зашифрованное сообщение
    public class Messages   
    {
        //Текст сообщения
        public string [] text { get; set; }
        //Дата и время отправки сообщения
        public DateTime dateTime { get; set; }
        public Messages(string [] text, DateTime dateTime)
        {
            this.text = text;
            this.dateTime = dateTime;
        }
        
    }
    //Содержание сообщения для клиентов
    public class MessageContent
    {
        //Текст сообщения
        public string Text { get; set; }
        //Дата и время отправки сообщения
        public DateTime Time { get; set; }
        //Логин отправителя
        public string Login { get; set; }
    }
    
}
