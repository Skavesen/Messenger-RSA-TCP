using MessengerClient.DBMS;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        //Поля для логина клиента
        public static string username = null;
        public AuthWindow()
        {
            InitializeComponent();
        }
        //Захват мышкой и перемещение по экрану
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //Закрытие приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Переход к другому окну
        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            //Получение логина
            string login = Login.Text.Trim();
            //Получение пароля
            string pass = Pass.Password.Trim(); 
            //Проверка пользователя
            Check(login, pass);
        }
        //Авторизация пользователя
        public void Check(string login,string pass)
        {
            //Проверка условий логина
            if (login.Length <= 3)
            {
                Login.ToolTip = "Имя должно быть длинее(минимум 3 буквы)";
            }
            //Проверка условий пароля
            else if (pass.Length < 5)
            {
                Pass.ToolTip = "Пароль должен быть длиннее";
            }
            //Если поля корректны
            else
            {
                Login.ToolTip = "";
                Pass.ToolTip = "";
                //Первоначальное значение пользователя
                User authUser = null;
                using (ApplicationContext applicationContext = new ApplicationContext())
                {
                    //Поиск пользователя в БД
                    authUser = applicationContext.Users.Where(cl => cl.Login == login && cl.Password == pass).FirstOrDefault();
                }
                //Если пользователь найден
                if (authUser != null)
                {
                    //Передача имени авторизованного пользователя
                    username = authUser.Login;
                    //переход на другое окно
                    new ApplicationViewModel(Server.Start.SearchIP(), 5050);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                //Сообщение об ошибке
                else
                    MessageBox.Show("Вы ввели что-то неккоректно");
            }
        }
        //Переход на другое окно
        private void Reg_Swipe_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow();
            registration.Show();
            this.Close();        
        }
        
    }
}
