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
    /// Логика взаимодействия для Timetable.xaml
    /// </summary>
    public partial class Timetable : Page
    {
        public List<string> DaysOfWeek { get; set; }

        public Timetable()
        {
            try
            {
                InitializeComponent();
                this.Loaded += timetable_Loaded;
                DaysOfWeekControl.ItemsSource = DaysOfWeek;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timetable_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadTimetable();
                LoadLocalizedDays();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        public void LoadLocalizedDays()
        {
            DaysOfWeek = new List<string>
    {
        Application.Current.TryFindResource("Monday") as string,
        Application.Current.TryFindResource("Tuesday") as string,
        Application.Current.TryFindResource("Wednesday") as string,
        Application.Current.TryFindResource("Thursday") as string,
        Application.Current.TryFindResource("Friday") as string,
        Application.Current.TryFindResource("Saturday") as string,
        Application.Current.TryFindResource("Sunday") as string

    };
        }


        private void LoadTimetable()
        {
            var all = DatabaseHelper.GetTimetable(); // получаем расписание
            var daysOfWeek = new[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

            DaysContainer.Items.Clear(); // Очистка контейнера, если уже что-то есть

            foreach (var day in daysOfWeek)
            {
                // Фильтрация по дню недели
                var entries = all.Where(e => e.DayOfWeek == day).ToList();

                // Создаём ItemsControl для этого дня недели
                var itemsControl = new ItemsControl
                {
                    ItemsSource = entries,
                    ItemTemplate = (DataTemplate)FindResource("TimetableCardTemplate"), // Ссылка на шаблон из Page.Resources
                    Background = Brushes.Transparent
                };

                // Оборачиваем в StackPanel или Border — по желанию
                var column = new StackPanel
                {
                    Margin = new Thickness(5)
                };

                column.Children.Add(new TextBlock
                {
                    //Text = day,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.White,
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 0, 0, 5),
                    TextAlignment = TextAlignment.Center
                });

                column.Children.Add(itemsControl);
                DaysContainer.Items.Add(column);
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
