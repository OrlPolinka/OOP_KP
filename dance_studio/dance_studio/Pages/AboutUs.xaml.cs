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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace dance_studio.Pages
{
    /// <summary>
    /// Логика взаимодействия для AboutUs.xaml
    /// </summary>
    public partial class AboutUs : Page
    {
        public AboutUs()
        {
            try
            {
                InitializeComponent();
                this.Loaded += AboutUs_Loaded;
                My_Scroll = this.FindName("MyScroll") as ScrollViewer;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AboutUs_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
            }
        }


        //private void AnimateScroll(double toValue)
        //{
        //    DoubleAnimation animation = new DoubleAnimation
        //    {
        //        To = toValue,
        //        Duration = TimeSpan.FromMilliseconds(300),
        //        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
        //    };

        //    AnimationClock clock = animation.CreateClock();
        //    MyScroll.ApplyAnimationClock(ScrollViewerHorizontalOffsetProperty, clock);
        //}

        //// Вспомогательное свойство для прокрутки
        //public static readonly DependencyProperty ScrollViewerHorizontalOffsetProperty =
        //    DependencyProperty.RegisterAttached("ScrollViewerHorizontalOffset", typeof(double), typeof(MainWindow),
        //        new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

        //private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is ScrollViewer scrollViewer)
        //    {
        //        scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        //    }
        //}

        public static ScrollViewer My_Scroll { get; set; } // Статическое свойство
        private void ScrollLeft(object sender, RoutedEventArgs e)
        {
            double targetOffset = MyScroll.HorizontalOffset - 300;
            if (targetOffset < 0) targetOffset = 0;
            Directions.AnimateScroll(My_Scroll, targetOffset);
        }

        private void ScrollRight(object sender, RoutedEventArgs e)
        {
            double targetOffset = MyScroll.HorizontalOffset + 300;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Seccion.IsClient)
            {
                // Открываем диалоговое окно для ввода отзыва
                AddReview addReview = new AddReview();
                bool? result = addReview.ShowDialog();

                if (result == true)
                {
                    // Получаем текст отзыва из окна
                    string reviewText = addReview.ReviewText;
                    string userName = Seccion.Username;

                    DatabaseHelper.AddReviewToDatabase(reviewText, userName);

                    LoadReviews();

                    // Добавляем новый отзыв в ScrollViewer
                    //AddReviewToScrollViewer(reviewText);
                }
            }
            else
            {
                MessageBox.Show("Данная функция только для клиентов");
            }
        }

        public void AddReviewToScrollViewer(string reviewText, string userName)
        {
            // Создаем новый элемент отзыва
            Border newReviewBorder = new Border
            {
                Width = 300,
                Height = 200,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(20),
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Color.FromArgb(114, 255, 255, 255)) // #72FFFFFF
            };

            StackPanel reviewStackPanel = new StackPanel { Margin = new Thickness(10) };

            TextBlock titleTextBlock = new TextBlock
            {
                Text = "отзыв",
                FontSize = 24,
                Foreground = new SolidColorBrush(Color.FromArgb(139, 0, 0, 0)) // #8B0000
            };

            TextBlock userNameTextBlock = new TextBlock
            {
                Text = "Пользователь: " + userName,  // Отображаем имя пользователя
                FontSize = 14,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 5, 0, 0)
            };

            TextBlock contentTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Text = reviewText,
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 0)
            };

            reviewStackPanel.Children.Add(titleTextBlock);
            reviewStackPanel.Children.Add(userNameTextBlock);  // Добавляем имя пользователя
            reviewStackPanel.Children.Add(contentTextBlock);

            newReviewBorder.Child = reviewStackPanel;

            // Добавляем новый отзыв в StackPanel в ScrollViewer
            this.ReviewStackPanel.Children.Add(newReviewBorder);
        }


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DANCE_STUDIO_BD"].ConnectionString;

        public void LoadReviews()
        {


            string query = "SELECT REV_TEXT, USER_NAME FROM Reviews ORDER BY DATE_CREATED DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    this.ReviewStackPanel.Children.Clear(); // Очистка текущих отзывов на экране

                    try
                    {

                        while (reader.Read())
                        {
                            string reviewText = reader.GetString(0); // Чтение текста отзыва
                            string userName = reader.GetString(1);  // Чтение имени пользователя

                            // Добавляем каждый отзыв в ScrollViewer
                            AddReviewToScrollViewer(reviewText, userName);
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
                    }
                }
            }
        }
    }
}
