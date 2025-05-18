using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        private string username;
        private string role;

        public Account()
        {
            try
            {
                InitializeComponent();
                this.Loaded += Subscriptions_Loaded;
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

        private void ApplyTheme(string themePath)
        {
            var themeDictionary = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.RelativeOrAbsolute)
            };

            // Находим текущую тему
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var existingTheme = dictionaries
                .FirstOrDefault(d => d.Source != null &&
                    (d.Source.OriginalString.Contains("LightTheme.xaml") || d.Source.OriginalString.Contains("DarkTheme.xaml")));

            if (existingTheme != null)
                dictionaries.Remove(existingTheme);

            dictionaries.Add(themeDictionary);
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/LightTheme.xaml");
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme("Resources/DarkTheme.xaml");
        }


        private void Subscriptions_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadSubscriptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DANCE_STUDIO_BD"].ConnectionString;


        private void LoadSubscriptions()
        {

            // Загружаем подписки по текущему пользователю
            try
            {

                try
                {
                    string query = "SELECT USER_NAME, PASSWORD, PHONE, EMAIL, NOTIFICATIONS FROM CLIENTS WHERE USER_NAME = @username";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", Seccion.Username);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["USER_NAME"] == DBNull.Value ? "" : reader["USER_NAME"].ToString();
                                string password = reader["PASSWORD"] == DBNull.Value ? "" : reader["PASSWORD"].ToString();
                                string phone = reader["PHONE"] == DBNull.Value ? "" : reader["PHONE"].ToString();
                                string email = reader["EMAIL"] == DBNull.Value ? "" : reader["EMAIL"].ToString();
                                string notifications = reader["NOTIFICATIONS"] == DBNull.Value ? "" : reader["NOTIFICATIONS"].ToString();

                                UsernameBox.Text = name;
                                PasswordBox.Text = password;
                                PhoneBox.Text = phone;
                                EmailBox.Text = email;
                                NotificationsBox.Text = notifications;

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении имени: " + ex.Message);
                }

                var subscriptions = DatabaseHelper.GetUserSubscriptions(Seccion.Username);
                SubscriptionList.ItemsSource = subscriptions;

                var records = DatabaseHelper.LoadClientRecords(Seccion.Username);
                RecordsList.ItemsSource = records;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }

        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            // Получаем новое имя из TextBox
            string newUsername = UsernameBox.Text.Trim();
            string password = PasswordBox.Text.Trim();
            string phone = PhoneBox.Text.Trim();
            string email = EmailBox.Text.Trim();


            // Проверяем, чтобы имя не было пустым
            if (string.IsNullOrEmpty(newUsername))
            {
                MessageBox.Show("Имя пользователя не может быть пустым!");
                return;
            }

            // Получаем текущего пользователя
            string currentUsername = Seccion.Username; // Предполагаем, что текущий пользователь хранится в Seccion.Username

            // Обновляем имя пользователя в базе данных
            bool isUpdated = DatabaseHelper.UpdateUsernameInDatabase(currentUsername, newUsername, password, phone, email);

            if (isUpdated)
            {
                // Если обновление прошло успешно, обновляем данные в Seccion
                Seccion.Username = newUsername;
                MessageBox.Show("Личные данные успешно обновлены!");

                // Можно обновить интерфейс, если нужно
                UsernameBox.Text = newUsername;
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении личных данных!");
            }
        }



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
                if (Seccion.IsClient)
                {
                    NavigationService?.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Данная страница только для клиентов.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seccion.IsClient)
                {
                    NavigationService?.Navigate(new Uri("Pages/Account.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Данная страница только для клиентов.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
