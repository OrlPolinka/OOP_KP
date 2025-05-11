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
    /// Логика взаимодействия для AdminSubscriptions.xaml
    /// </summary>
    public partial class AdminSubscriptions : Page
    {
        public AdminSubscriptions()
        {
            try
            {
                InitializeComponent();
                this.Loaded += SubsCards_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SubsCards_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadSubscriptionCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }


        public void LoadSubscriptionCards()
        {
            var subscriptions = DatabaseHelper.GetAbonements();
            SubscriptionItemsControl.ItemsSource = subscriptions;
        }


        // Обработчик кнопки "Удалить" в ListBox
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Abonements abonement)
            {
                var result = MessageBox.Show($"Удалить абонемент '{abonement.Title}'?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = DatabaseHelper.DeleteAbonementByTitle(abonement.Title);
                    if (deleted)
                    {
                        MessageBox.Show("Абонемент удалён.");
                        LoadSubscriptionCards();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении.");
                    }
                }
            }
        }



        private void ClearForm()
        {
            TitleTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            StyleTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            StyleTextBox.Clear();
            PriceTextBox.Clear();
        }

        // Обработчик кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            string style = StyleTextBox.Text;
            string price = PriceTextBox.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Сохранение новости в базу данных
            bool isSuccess = DatabaseHelper.AddAbonement(title, description, price, style);

            if (isSuccess)
            {
                MessageBox.Show("Абонемент успешно сохранен!");
                LoadSubscriptionCards(); // Перезагружаем список новостей
                ClearForm();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при сохранении новости.");
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
                NavigationService?.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
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
