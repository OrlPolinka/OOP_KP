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
    /// Логика взаимодействия для AdminTrainers.xaml
    /// </summary>
    public partial class AdminTrainers : Page
    {
        public AdminTrainers()
        {
            try
            {
                InitializeComponent();
                this.Loaded += Trainers_Loaded;
                My_Scroll = this.FindName("MyScroll") as ScrollViewer;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Trainers_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadTrainers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string fio = FIOTextBox.Text;
            string description = DescriptionTextBox.Text;
            string fioEn = FIOENTextBox.Text;
            string descriptionEn = DescriptionENTextBox.Text;
            string imagePath = ImagePathTextBlock.Text;

            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(fioEn) || string.IsNullOrWhiteSpace(descriptionEn))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Сохранение новости в базу данных
            bool isSuccess = DatabaseHelper.AddTrainersToDB(fio, description, imagePath, fioEn, descriptionEn);

            if (isSuccess)
            {
                MessageBox.Show("Тренер успешно добавлен!");
                LoadTrainers(); // Перезагружаем список новостей
                ClearForm();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при сохранении тренера.");
            }
        }

        private void ClearForm()
        {
            FIOTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            ImagePathTextBlock.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            FIOTextBox.Clear();
            DescriptionTextBox.Clear();
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int trainerId;

                try
                {
                    trainerId = Convert.ToInt32(button.Tag); // безопасно, если Tag — int, long, string с числом и т.д.
                }
                catch
                {
                    MessageBox.Show("Некорректный ID тренера.");
                    return;
                }

                var result = MessageBox.Show("Удалить этого тренера?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    bool deleted = DatabaseHelper.DeleteTrainersFromDatabase(trainerId);
                    if (deleted)
                    {
                        MessageBox.Show("Тренер удален.");
                        LoadTrainers(); // обновление списка
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении.");
                    }
                }
            }
        }





        public void LoadTrainers()
        {
            var trainers = DatabaseHelper.GetTrainers();
            TrainersItemsControl.ItemsSource = trainers;
        }

        //private void AnimateScroll(double toValue)
        //{
        //    DoubleAnimation animation = new DoubleAnimation
        //    {
        //        To = toValue,
        //        Duration = TimeSpan.FromMilliseconds(500),
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
