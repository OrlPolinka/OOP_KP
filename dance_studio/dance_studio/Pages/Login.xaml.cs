using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (LanguageComboBox.SelectedItem is ComboBoxItem item && item.Tag is string lang)
                {
                    ((App)Application.Current).ChangeLanguage(lang);

                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки или вывод в консоль
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();


            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            string role = (username == "Орловская Полина" && password == "admin") ? "Администратор" : "Клиент";

            if (LoginRadio.IsChecked == true)
            {
                //вход
                if (DatabaseHelper.CheckUserExists(username, password, role))
                {
                    MessageBox.Show($"Вход выполнен как {role}: {username}");

                    Seccion.Username = username;
                    Seccion.Role = role;
                    NavigationService?.Navigate(new Uri("Pages/Main.xaml", UriKind.Relative));


                }
                else
                {
                    MessageBox.Show("Неверные имя пользователя или пароль.");
                }
            }
            else
            {
                //регистрация
                if (password == "admin")
                {
                    MessageBox.Show($"Данный пароль не может быть использован для регистрации");

                }
                else if (DatabaseHelper.RegisterUser(username, password, role))
                {
                    MessageBox.Show($"Успешная регистрация как {role}: {username}");
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.");
                }
            }

        }



        private void GoToMain(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Main.xaml", UriKind.Relative));
        }

        private void GoToNews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/News.xaml", UriKind.Relative));
        }
        private void GoToDirections(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Directions.xaml", UriKind.Relative));
        }

        private void GoToTrainers(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Trainers.xaml", UriKind.Relative));
        }

        private void GoToTimetable(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Timetable.xaml", UriKind.Relative));
        }

        private void GoToSubscriptions(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Subscriptions.xaml", UriKind.Relative));
        }

        private void GoToContacts(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Contacts.xaml", UriKind.Relative));
        }

        private void GoToAboutUs(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/AboutUs.xaml", UriKind.Relative));
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
        }
        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
        }
    }
}
