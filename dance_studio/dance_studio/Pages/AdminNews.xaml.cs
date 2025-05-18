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
using System.Data.Entity;
using System.Windows.Shapes;

namespace dance_studio.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminNews.xaml
    /// </summary>
    public partial class AdminNews : Page
    {
        public static readonly RoutedUICommand SaveCommand = new RoutedUICommand(
        "Save",
        "SaveCommand",
        typeof(AdminNews));


        private readonly DanceStudioContext _context = new DanceStudioContext();
        private int? _editingNewsId = null;
        public AdminNews()
        {
            try
            {
                InitializeComponent();
                this.Loaded += News_Loaded;// Привязка команды к обработчику
                CommandBindings.Add(new CommandBinding(SaveCommand, SaveCommand_Executed, SaveCommand_CanExecute));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveButton_Click(sender, e);  // повторно используем существующий метод сохранения
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Логика для активации команды (например, проверка заполненности полей)
            bool canSave = !string.IsNullOrEmpty(TitleTextBox.Text) &&
                           !string.IsNullOrEmpty(DescriptionTextBox.Text) &&
                           PublishDatePicker.SelectedDate.HasValue;
            e.CanExecute = canSave;
        }

        private async void News_Loaded(object sender, RoutedEventArgs e)
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
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string titleRu = TitleTextBox.Text.Trim();
            string titleEn = TitleTextBoxEn.Text.Trim();
            string descRu = DescriptionTextBox.Text.Trim();
            string descEn = DescriptionTextBoxEn.Text.Trim();
            DateTime? publishDate = PublishDatePicker.SelectedDate.Value;
            string imagePath = ImagePathTextBlock.Text;

            if (string.IsNullOrEmpty(titleRu) || string.IsNullOrEmpty(descRu) || !publishDate.HasValue || string.IsNullOrEmpty(titleEn) || string.IsNullOrEmpty(descEn))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }
            //try
            //{
            //    var news = new Newss
            //    {
            //        PublishDate = PublishDatePicker.SelectedDate.Value,
            //        ImagePath = ImagePathTextBlock.Text,
            //        Status = "Active",
            //        Localizations =
            //    {
            //        new NewsLocalization
            //        {
            //            LanguageCode = "ru",
            //            Title = TitleTextBox.Text.Trim(),
            //            Description = DescriptionTextBox.Text.Trim()
            //        },
            //        new NewsLocalization
            //        {
            //            LanguageCode = "en",
            //            Title = TitleTextBoxEn.Text.Trim(),
            //            Description = DescriptionTextBoxEn.Text.Trim()
            //        }
            //    }
            //    };

            //    _context.News.Add(news);
            //    await _context.SaveChangesAsync();

            //    MessageBox.Show("Новость успешно сохранена!");
            //    await LoadNewsListAsync();
            //    ClearForm();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            //}


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
            TitleTextBoxEn.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            DescriptionTextBoxEn.Text = string.Empty;
            PublishDatePicker.SelectedDate = null;
            ImagePathTextBlock.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            ClearForm();
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
        private async Task LoadNewsListAsync()
        {
            try
            {
                var newsList = await _context.News
                     .Include("Localizations")
                    .OrderByDescending(n => n.PublishDate)
                    .ToListAsync();

                NewsListBox.ItemsSource = newsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке новостей: {ex.Message}");
            }
        }

        private void LoadNewsList()
        {
            var newsList = DatabaseHelper.GetNewsFromDatabase();
            NewsListBox.ItemsSource = newsList;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //if (!(sender is Button button) || !(button.Tag is int newsId)) return;

            //var result = MessageBox.Show("Удалить эту новость?", "Подтверждение", MessageBoxButton.YesNo);
            //if (result != MessageBoxResult.Yes) return;

            //try
            //{
            //    var news = await _context.News
            //        .Include(n => n.Localizations)
            //        .FirstOrDefaultAsync(n => n.Id == newsId);

            //    if (news != null)
            //    {
            //        _context.NewsLocalizations.RemoveRange(news.Localizations);
            //        _context.News.Remove(news);
            //        await _context.SaveChangesAsync();

            //        MessageBox.Show("Новость удалена");
            //        await LoadNewsListAsync();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            //}
            //    if (sender is Button button)
            //    {
            //        int newsId;

            //        try
            //        {
            //            newsId = Convert.ToInt32(button.Tag); // безопасно, если Tag — int, long, string с числом и т.д.
            //        }
            //        catch
            //        {
            //            MessageBox.Show("Некорректный ID новости.");
            //            return;
            //        }
            //        Console.WriteLine($"Пытаемся удалить новость с ID = {newsId}"); // Вывод в отладку

            //        var result = MessageBox.Show("Удалить эту новость?", "Подтверждение", MessageBoxButton.YesNo);
            //        if (result == MessageBoxResult.Yes)
            //        {

            //            bool deleted = DatabaseHelper.DeleteNewsFromDatabase(newsId);
            //            if (deleted)
            //            {
            //                MessageBox.Show("Новость удалена.");
            //                LoadNewsList(); // обновление списка
            //            }
            //            else
            //            {
            //                MessageBox.Show("Ошибка при удалении.");
            //            }
            //        }
            //    }
            //if (sender is Button button && button.Tag is int newsId)



            if (sender is Button button && button.Tag != null && int.TryParse(button.Tag.ToString(), out int newsId))
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
