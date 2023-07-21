using MessengerClient.DBMS;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        ApplicationContext applicationContext;
        public RegistrationWindow()
        {
            InitializeComponent();
            applicationContext = new ApplicationContext();
        }
        //Захват мышкой и перемещение
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //Выход из приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Переход к другому окну если аккаунт уже есть
        private void Swipe_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow auth = new AuthWindow();
            auth.Show();
            this.Close();
        }
        //Кнопка регестрации
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            //Получение логина
            string login = Login.Text.Trim();
            //Получение пароля
            string pass1 = Pass1.Password.Trim(); ;
            //Повтор пароля
            string pass2 = Pass2.Password.Trim(); ;
            CheckandRegistr(login, pass1, pass2);
           
        }
        //Проверка полей и регестрация в приложении
        public void CheckandRegistr(string login,string pass1,string pass2)
        {
            //Провекра условий логина
            if (login.Length <= 3)
            {
                Login.ToolTip = "Имя должно быть длинее(минимум 3 буквы)";
                MessageBox.Show("Проверьте введенные данные");
            }
            //Провекра условий пароля
            else if (pass1.Length < 5)
            {
                Pass1.ToolTip = "Пароль должен быть длиннее 5 символов";
                MessageBox.Show("Проверьте введенные данные");
            }
            //Проверка на совпадение паролей
            else if (pass1 != pass2)
            {
                Pass2.ToolTip = "Пароли не совпадают";
                MessageBox.Show("Проверьте введенные данные");
            }
            //Если все поля корректны
            else
            {
                Login.ToolTip = "";
                Pass1.ToolTip = "";
                Pass2.ToolTip = "";
                User authUser = null;
                using (ApplicationContext applicationContext = new ApplicationContext())
                {
                    //Поиск пользователя в БД
                    authUser = applicationContext.Users.Where(cl => cl.Login == login && cl.Password == pass1).FirstOrDefault();
                }
                //Если пользователь найден
                if (authUser != null)
                {
                    MessageBox.Show("Такой пользователь уже есть");
                }
                else
                {
                    //Регистрация нового пользователя
                    User user = new User(login, pass1);
                    //Добавление клиента в список
                    applicationContext.Users.Add(user);
                    //Сохранение изменений
                    applicationContext.SaveChanges();
                    //Сообщение об успешной регестрации
                    MessageBox.Show("Регистрация пройдена успешно!");
                    //переход на другое окно
                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    this.Close();
                }
            }
        }
    }
}

