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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Seccion.IsClient)
            {
                // Открываем диалоговое окно для ввода отзыва
                var addReviewWindow = new AddReview();
                if (addReviewWindow.ShowDialog() == true)
                {
                    DatabaseHelper.AddReviewToDatabase(
                        addReviewWindow.ReviewText,
                        Seccion.Username,
                        addReviewWindow.Rating
                    );
                    // Обновить список отзывов
                    LoadReviews();
                }
            }
            else
            {
                MessageBox.Show("Данная функция только для клиентов");
            }
        }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DANCE_STUDIO_BD"].ConnectionString;



        // Удаление отзыва
        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Review review)
            {
                try
                {
                    if (DatabaseHelper.DeleteReview(review.ReviewId))
                    {
                        // Обновляем список отзывов
                        LoadReviews();
                        MessageBox.Show("Отзыв успешно удален");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении отзыва: {ex.Message}");
                }
            }
        }

        // Редактирование отзыва
        private void EditReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Review review)
            {
                // Открываем окно редактирования
                var editWindow = new EditReviewWindow(review);
                if (editWindow.ShowDialog() == true)
                {
                    // Обновляем список после редактирования
                    LoadReviews();
                }
            }
        }

        private void LoadReviews()
        {
            try
            {
                ReviewStackPanel.Children.Clear();

                var reviews = DatabaseHelper.GetReviews();
                if (reviews == null || reviews.Count == 0)
                {
                    // Добавляем сообщение, если отзывов нет
                    ReviewStackPanel.Children.Add(new TextBlock
                    {
                        Text = "Пока нет отзывов",
                        FontSize = 16,
                        Foreground = Brushes.White,
                        Margin = new Thickness(10)
                    });
                    return;
                }

                foreach (var review in reviews)
                {
                    var reviewItem = new Border
                    {
                        CornerRadius = new CornerRadius(10),
                        Background = new SolidColorBrush(Colors.White),
                        Margin = new Thickness(10),
                        Padding = new Thickness(15),
                        Width = 350,
                        MaxHeight = 200, // Ограничиваем высоту отзыва
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(1)
                    };

                    // Главный контейнер с прокруткой
                    var scrollViewer = new ScrollViewer
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
                    };


                    var stackPanel = new StackPanel();

                    // Добавляем элементы отзыва
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = $"Пользователь: {review.UserName}",
                        FontWeight = FontWeights.Bold,
                        FontSize = 16
                    });

                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = $"Оценка: {new string('★', review.Rating)}{new string('☆', 5 - review.Rating)}",
                        Margin = new Thickness(0, 5, 0, 0),
                        Foreground = Brushes.Gold,
                        FontSize = 14
                    });

                    // Текст отзыва с возможностью прокрутки
                    var reviewTextBlock = new TextBlock
                    {
                        Text = review.Text,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 10, 0, 0),
                        FontSize = 14
                    };
                    stackPanel.Children.Add(reviewTextBlock);


                    // Добавляем кнопки (если это отзыв текущего пользователя)
                    if (review.EditButtonsVisibility == Visibility.Visible || !Seccion.IsClient)
                    {
                        
                        var buttonsPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(0, 10, 0, 0)
                            
                        };

                        // Кнопка "Изменить" показываем только автору отзыва
                        if (review.EditButtonsVisibility == Visibility.Visible)
                        {
                            var editButton = new Button
                            {
                                Content = "Изменить",
                                Margin = new Thickness(0, 0, 10, 0),
                                Padding = new Thickness(5),
                                DataContext = review,
                                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF75555")),
                                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B0000")),
                                BorderThickness = new Thickness(1)
                            };
                            editButton.Click += EditReview_Click;
                            buttonsPanel.Children.Add(editButton);
                        }

                        var deleteButton = new Button
                        {
                            Content = "Удалить",
                            Padding = new Thickness(5),
                            DataContext = review,
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF75555")),
                            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B0000")),
                            BorderThickness = new Thickness(1)
                        };
                        deleteButton.Click += DeleteReview_Click;

                        buttonsPanel.Children.Add(deleteButton);

                        stackPanel.Children.Add(buttonsPanel);
                    }
                    scrollViewer.Content = stackPanel;
                    reviewItem.Child = scrollViewer;
                    ReviewStackPanel.Children.Add(reviewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке отзывов: " + ex.Message);
            }
        }
    }
}
