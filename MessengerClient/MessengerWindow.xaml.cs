using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;
using Server;
using System.Windows.Media;

namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Пререход к последнему сообщению(скрол вниз автом)
            Loaded+=(s,e)=>((INotifyCollectionChanged)Items.ItemsSource).CollectionChanged+=(q,z)=>Scroll.ScrollToEnd();
            DataContext = new ApplicationViewModel(Start.SearchIP(),5050);
        }
        //Закрытие окна приложения
         public void CloseWindow(object sender, RoutedEventArgs e)
         {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
            
         }
        //Захват мышкой и передвижение по экрану
        public void DrivePanel(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //Дизайн по умолчанию
        private void FirstD_Click(object sender, RoutedEventArgs e)
        {
            //Переменная для конвертации цвета
            var bc = new BrushConverter();
            //Изменение цвета панелей
            HorizontalPanel.Background = (Brush)bc.ConvertFrom("#FF563096");
            VerticalPanel.Background = (Brush)bc.ConvertFrom("#FF563096");
            //Изменение цвета кнопок
            Dis.Background = (Brush)bc.ConvertFrom("#FF5A3096");
            Con.Background = (Brush)bc.ConvertFrom("#FF5A3096");
            First.Background = (Brush)bc.ConvertFrom("#FF673AB7");
            Second.Background = (Brush)bc.ConvertFrom("#FF673AB7");
            Theard.Background = (Brush)bc.ConvertFrom("#FF673AB7");
            First.BorderBrush = (Brush)bc.ConvertFrom("#FF673AB7");
            Second.BorderBrush = (Brush)bc.ConvertFrom("#FF673AB7");
            Theard.BorderBrush = (Brush)bc.ConvertFrom("#FF673AB7");
            //Изменение цвета заднего фона
            Main.Background = (Brush)bc.ConvertFrom("#FF2F2B42");
            //Изменение цвета кнопки отправки
            Send.Background = (Brush)bc.ConvertFrom("#FF5A3096");
            Send.BorderBrush = (Brush)bc.ConvertFrom("#FF673AB7");
            //Изменение цвета текстового поля
            MessageBox.Background = (Brush)bc.ConvertFrom("#FF393251");
            //Изменение цвета кнопки закрыть
            Close.Background = (Brush)bc.ConvertFrom("#FF2F2B42");
            Close.BorderBrush = (Brush)bc.ConvertFrom("#FF673AB7");
        }
        //Второй вариант дизайна
        private void SecondD_Click(object sender, RoutedEventArgs e)
        {
            //Переменная для конвертации цвета
            var bc = new BrushConverter();
            //Изменение цвета панелей
            HorizontalPanel.Background=(Brush)bc.ConvertFrom("#000033");
            VerticalPanel.Background = (Brush)bc.ConvertFrom("#000033");
            //Изменение цвета кнопок
            Dis.Background = (Brush)bc.ConvertFrom("#000033");
            Con.Background = (Brush)bc.ConvertFrom("#000033");
            First.Background = (Brush)bc.ConvertFrom("#000033");
            Second.Background = (Brush)bc.ConvertFrom("#000033");
            Theard.Background = (Brush)bc.ConvertFrom("#000033");
            First.BorderBrush = (Brush)bc.ConvertFrom("#3399ff");
            Second.BorderBrush = (Brush)bc.ConvertFrom("#3399ff");
            Theard.BorderBrush = (Brush)bc.ConvertFrom("#3399ff");
            //Изменение цвета заднего фона
            Main.Background = (Brush)bc.ConvertFrom("#000000");
            //Изменение цвета кнопки отправки
            Send.Background = (Brush)bc.ConvertFrom("#000033");
            Send.BorderBrush = (Brush)bc.ConvertFrom("#3399ff");
            //Изменение цвета текстового поля
            MessageBox.Background = (Brush)bc.ConvertFrom("#333333");
            //Изменение цвета кнопки закрыть
            Close.Background = (Brush)bc.ConvertFrom("#000033");
            Close.BorderBrush = (Brush)bc.ConvertFrom("#3399ff");
            
        }
        //Третий вариант дизайна
        private void TheardD_Click(object sender, RoutedEventArgs e)
        {
            //Переменная для конвертации цвета
            var bc = new BrushConverter();
            //Изменение цвета панелей
            HorizontalPanel.Background = (Brush)bc.ConvertFrom("#660033");
            VerticalPanel.Background = (Brush)bc.ConvertFrom("#660033");
            //Изменение цвета кнопок
            Dis.Background = (Brush)bc.ConvertFrom("#660033");
            Con.Background = (Brush)bc.ConvertFrom("#660033");
            First.Background = (Brush)bc.ConvertFrom("#660033");
            Second.Background = (Brush)bc.ConvertFrom("#660033");
            Theard.Background = (Brush)bc.ConvertFrom("#660033");
            First.BorderBrush = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second.BorderBrush = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Theard.BorderBrush = (Brush)bc.ConvertFrom("#FFFFFFFF");
            //Изменение цвета заднего фона
            Main.Background = (Brush)bc.ConvertFrom("#FF150115");
            //Изменение цвета кнопки отправки
            Send.Background = (Brush)bc.ConvertFrom("#660033");
            Send.BorderBrush = (Brush)bc.ConvertFrom("#FFFFFFFF");
            //Изменение цвета текстового поля
            MessageBox.Background = (Brush)bc.ConvertFrom("#333333");
            //Изменение цвета кнопки закрыть
            Close.Background = (Brush)bc.ConvertFrom("#660033");
            Close.BorderBrush = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }
    }
}

