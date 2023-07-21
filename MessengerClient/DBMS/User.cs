namespace MessengerClient.DBMS
{
    //Класс модели
    class User
    {
        //Id клиента(уникальный)
        public int id { get; set; }
        //Поля логина и пароля
        private string login, password;
        public string Login
        {
            get => login;set
            {
                login = value;
            }
        }
        public string Password
        {
            get => password; set
            {
                password = value;
            }
        }
        //Конструктор по умолчанию
        public User () { }
        //Конструктор для передачи логина и пароля
        public User(string login,string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
