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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dance_studio.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminDirections.xaml
    /// </summary>
    public partial class AdminDirections : Page
    {
        private UndoRedoManager _undoRedoManager = new UndoRedoManager();
        public AdminDirections()
        {
            try
            {
                InitializeComponent();
                this.Loaded += Styles_Loaded;
                My_Scroll = this.FindName("MyScroll") as ScrollViewer;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Styles_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadStyles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string style = StyleTextBox.Text;
            string styleEn = StyleEnTextBox.Text;
            string description = DescriptionTextBox.Text;
            string descriptionEn = DescriptionEnTextBox.Text;

            if (string.IsNullOrWhiteSpace(style) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(styleEn) || string.IsNullOrWhiteSpace(descriptionEn))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Сохранение новости в базу данных
            bool isSuccess = DatabaseHelper.AddStyle(style, description, styleEn, descriptionEn);

            if (isSuccess)
            {
                MessageBox.Show("Стиль успешно добавлен!");
                LoadStyles(); // Перезагружаем список новостей
                ClearForm();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при сохранении стиля.");
            }
        }

        private void ClearForm()
        {
            StyleTextBox.Text = string.Empty;
            StyleEnTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            DescriptionEnTextBox.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            ClearForm();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int styleId;

                try
                {
                    styleId = Convert.ToInt32(button.Tag); // безопасно, если Tag — int, long, string с числом и т.д.
                }
                catch
                {
                    MessageBox.Show("Некорректный ID стиля.");
                    return;
                }

                var result = MessageBox.Show("Удалить этот стиль?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    bool deleted = DatabaseHelper.DeleteStyleFromDatabase(styleId);
                    if (deleted)
                    {
                        MessageBox.Show("Стиль удален.");
                        LoadStyles(); // обновление списка
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении.");
                    }
                }
            }
        }





        public void LoadStyles()
        {
            var styles = DatabaseHelper.GetStyles();
            StylesItemsControl.ItemsSource = styles;
        }

        public static ScrollViewer My_Scroll { get; set; } // Статическое свойство

        private void ScrollLeft(object sender, RoutedEventArgs e)
        {
            double targetOffset = MyScroll.HorizontalOffset - 470;
            if (targetOffset < 0) targetOffset = 0;
            Directions.AnimateScroll(My_Scroll, targetOffset);
        }

        private void ScrollRight(object sender, RoutedEventArgs e)
        {
            double targetOffset = MyScroll.HorizontalOffset + 470;
            double maxOffset = MyScroll.ScrollableWidth;
            if (targetOffset > maxOffset) targetOffset = maxOffset;
            Directions.AnimateScroll(My_Scroll, targetOffset);
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
