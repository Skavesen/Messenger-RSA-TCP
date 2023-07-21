namespace ClientsInfo
{
    public static class News
    {
        //Возвращает текст подлкюченного клиента, передает логин и время отправки
        public static MessageContent messageContent (this MessageSending messageSending,string text)
        {
            return new MessageContent() { Text = text, Login = messageSending.clientsConnected.Login, Time = messageSending.messages.dateTime };
        }
    }
}
