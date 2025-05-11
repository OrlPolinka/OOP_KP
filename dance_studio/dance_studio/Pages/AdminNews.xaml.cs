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
    /// Логика взаимодействия для AdminNews.xaml
    /// </summary>
    public partial class AdminNews : Page
    {
        public AdminNews()
        {
            try
            {
                InitializeComponent();
                this.Loaded += News_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void News_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadNewsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        // Обработчик кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string titleRu = TitleTextBox.Text.Trim();
            string titleEn = TitleTextBoxEn.Text.Trim();
            string descRu = DescriptionTextBox.Text.Trim();
            string descEn = DescriptionTextBoxEn.Text.Trim();
            DateTime? publishDate = PublishDatePicker.SelectedDate.Value;
            string imagePath = ImagePathTextBlock.Text;

            if (string.IsNullOrEmpty(titleRu) || string.IsNullOrEmpty(descRu) || !publishDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Генерируем уникальные ключи
            string guid = Guid.NewGuid().ToString("N");
            string titleKey = $"news_title_{guid}";
            string descKey = $"news_description_{guid}";

            bool saved = DatabaseHelper.SaveNewsToDatabaseWithLocalization(titleRu, titleEn, descRu, descEn, publishDate.Value, imagePath);


            if (saved)
            {
                MessageBox.Show("Новость успешно добавлена!");
                LoadNewsList(); // Обновляем список
            }

            DatabaseHelper.AddNotifications(titleRu);

        }

        private void ClearForm()
        {
            TitleTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            PublishDatePicker.SelectedDate = null;
            ImagePathTextBlock.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            PublishDatePicker.SelectedDate = null;
            ImagePathTextBlock.Text = string.Empty;
        }

        // Обработчик кнопки "Загрузить изображение"
        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBlock.Text = openFileDialog.FileName;
            }
        }

        // Метод для загрузки новостей из базы данных и отображения их в ListBox
        private void LoadNewsList()
        {
            var newsList = DatabaseHelper.GetNewsFromDatabase();
            NewsListBox.ItemsSource = newsList;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int newsId)
            {
                var result = MessageBox.Show("Удалить эту новость?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = DatabaseHelper.DeleteNewsFromDatabase(newsId);
                    if (deleted)
                    {
                        MessageBox.Show("Новость удалена.");
                        LoadNewsList(); // обновление списка
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении.");
                    }
                }
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
