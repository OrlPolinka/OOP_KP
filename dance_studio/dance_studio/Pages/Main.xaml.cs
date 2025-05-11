using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dance_studio.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public bool isAdmin;
        public bool isClient;
        public Main()
        {
            try
            {
                InitializeComponent();
                this.Loaded += Window_Loaded;

                // Обновите видимость элементов меню
                //UpdateMenuVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Window_Loaded();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        public void Window_Loaded()
        {
            // Ищем меню
            var menu = this.FindName("MainMenu") as Menu;
            if (menu != null)
            {
                //Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/dancer_icon.ico"));
            }
        }
        public Visibility SignUpVisibility { get; set; }


        private void GoToMain(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService?.Navigate(new Uri("Pages/Main.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToNews(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    // Если пользователь администратор, показываем страницу с расписанием для админов
                    NavigationService?.Navigate(new Uri("Pages/News.xaml", UriKind.Relative));
                }
                else
                {
                    // Для клиентов показываем обычное расписание
                    NavigationService?.Navigate(new Uri("Pages/AdminNews.xaml", UriKind.Relative));
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToDirections(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    NavigationService?.Navigate(new Uri("Pages/Directions.xaml", UriKind.Relative));
                }
                else
                {

                    NavigationService?.Navigate(new Uri("Pages/AdminDirections.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToTrainers(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    NavigationService?.Navigate(new Uri("Pages/Trainers.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService?.Navigate(new Uri("Pages/AdminTrainers.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToTimetable(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    // Если пользователь администратор, показываем страницу с расписанием для админов
                    NavigationService?.Navigate(new Uri("Pages/Timetable.xaml", UriKind.Relative));
                }
                else
                {
                    // Для клиентов показываем обычное расписание
                    NavigationService?.Navigate(new Uri("Pages/AdminTimetable.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToSubscriptions(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    // Если пользователь администратор, показываем страницу с расписанием для админов
                    NavigationService?.Navigate(new Uri("Pages/Subscriptions.xaml", UriKind.Relative));
                }
                else
                {
                    // Для клиентов показываем обычное расписание
                    NavigationService?.Navigate(new Uri("Pages/AdminSubscriptions.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToContacts(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService?.Navigate(new Uri("Pages/Contacts.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToAboutUs(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService?.Navigate(new Uri("Pages/AboutUs.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService?.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService?.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool _isNavigating = false;

        private void GoToAccount(object sender, RoutedEventArgs e)
        {
            // Защита от двойного нажатия
            if (_isNavigating) return;

            try
            {
                _isNavigating = true;

                if (Seccion.IsClient)
                {
                    // Вариант 1: Использование NavigationService с очисткой журнала
                    NavigationService?.Navigate(new Uri("Pages/Account.xaml", UriKind.Relative));

                    // Очищаем журнал навигации
                    while (NavigationService?.CanGoBack ?? false)
                    {
                        NavigationService.RemoveBackEntry();
                    }

                    /* 
                    // Вариант 2: Использование Frame (более надежный способ)
                    // В XAML: <Frame x:Name="MainFrame" NavigationUIVisibility="Hidden"/>
                    if (!(MainFrame.Content is Account))
                    {
                        MainFrame.Navigate(new Account());
                    }
                    */
                }
                else
                {
                    MessageBox.Show("Данная страница только для клиентов.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка перехода: {ex.Message}");
                // Логирование ошибки
                Debug.WriteLine($"[NAV ERROR] {DateTime.Now}: {ex}");
            }
            finally
            {
                _isNavigating = false;
            }
        }
    }
}
