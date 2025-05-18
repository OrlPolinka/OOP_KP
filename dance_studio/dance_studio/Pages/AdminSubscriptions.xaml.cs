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
    /// 

    // Интерфейс команды для Undo/Redo
    public interface ICommandAction
    {
        void Execute();
        void Undo();
    }


    // Команда добавления абонемента (добавляет в БД при Execute, удаляет при Undo)
    public class AddSubscriptionCommand : ICommandAction
    {
        private Abonements _abonement;

        public AddSubscriptionCommand(Abonements abonement)
        {
            _abonement = abonement;
        }

        public void Execute()
        {
            // Добавляем в БД
            DatabaseHelper.AddAbonement(
                _abonement.Title,
                _abonement.Description,
                _abonement.Price.ToString(),
                _abonement.Style,
                _abonement.TitleEn,
                _abonement.DescriptionEn,
                _abonement.StyleEn
            );
        }

        public void Undo()
        {
            // Удаляем из БД
            DatabaseHelper.DeleteAbonementByTitle(_abonement.Title);
        }
    }

    // Команда удаления абонемента (удаляет в Execute, добавляет назад при Undo)
    public class RemoveSubscriptionCommand : ICommandAction
    {
        private Abonements _abonement;

        public RemoveSubscriptionCommand(Abonements abonement)
        {
            _abonement = abonement;
        }

        public void Execute()
        {
            // Удаляем из БД
            DatabaseHelper.DeleteAbonementByTitle(_abonement.Title);
        }

        public void Undo()
        {
            // Добавляем обратно в БД
            DatabaseHelper.AddAbonement(
                _abonement.Title,
                _abonement.Description,
                _abonement.Price.ToString(),
                _abonement.Style,
                _abonement.TitleEn,
                _abonement.DescriptionEn,
                _abonement.StyleEn
            );
        }
    }

    // Менеджер Undo/Redo с двумя стеками
    public class UndoRedoManager
    {
        private Stack<ICommandAction> _undoStack = new Stack<ICommandAction>();
        private Stack<ICommandAction> _redoStack = new Stack<ICommandAction>();


        public void ExecuteCommand(ICommandAction command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Any())
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Any())
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }

        public bool CanUndo => _undoStack.Any();
        public bool CanRedo => _redoStack.Any();
    }


    public partial class AdminSubscriptions : Page
    {
        private UndoRedoManager _undoRedoManager = new UndoRedoManager();
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
                    try
                    {
                        var command = new RemoveSubscriptionCommand(abonement);
                        _undoRedoManager.ExecuteCommand(command);

                        MessageBox.Show("Абонемент удалён.");
                        LoadSubscriptionCards();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message);
                    }
                    //bool deleted = DatabaseHelper.DeleteAbonementByTitle(abonement.Title);
                    //if (deleted)
                    //{
                    //    MessageBox.Show("Абонемент удалён.");
                    //    LoadSubscriptionCards();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Ошибка при удалении.");
                    //}
                }
            }
        }



        private void ClearForm()
        {
            TitleTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            StyleTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;

            TitleEnTextBox.Text = string.Empty;
            DescriptionEnTextBox.Text = string.Empty;
            StyleEnTextBox.Text = string.Empty;
        }


        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всех полей формы
            ClearForm();
        }

        // Обработчик кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            string style = StyleTextBox.Text;
            string price = PriceTextBox.Text;
            string titleEn = TitleEnTextBox.Text;
            string descriptionEn = DescriptionEnTextBox.Text;
            string styleEn = StyleEnTextBox.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(style) || string.IsNullOrWhiteSpace(titleEn) || string.IsNullOrWhiteSpace(descriptionEn) || string.IsNullOrWhiteSpace(styleEn))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            if (!decimal.TryParse(price, out decimal priceValue) || priceValue <= 0)
            {
                MessageBox.Show("Введите корректную цену (только цифры, больше нуля)");
                return;
            }

            var abonement = new Abonements
            {
                Title = title,
                Description = description,
                Price = priceValue.ToString(),
                Style = style,
                TitleEn = titleEn,
                DescriptionEn = descriptionEn,
                StyleEn = styleEn
            };

            try
            {
                var command = new AddSubscriptionCommand(abonement);
                _undoRedoManager.ExecuteCommand(command);

                MessageBox.Show("Абонемент успешно сохранён.");
                LoadSubscriptionCards();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }


            //// Сохранение новости в базу данных
            //bool isSuccess = DatabaseHelper.AddAbonement(title, description, price, style, titleEn, descriptionEn, styleEn);

            //if (isSuccess)
            //{
            //    MessageBox.Show("Абонемент успешно сохранен!");
            //    LoadSubscriptionCards(); // Перезагружаем список новостей
            //    ClearForm();
            //}
            //else
            //{
            //    MessageBox.Show("Произошла ошибка при сохранении новости.");
            //}
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_undoRedoManager.CanUndo)
            {
                try
                {
                    _undoRedoManager.Undo();
                    LoadSubscriptionCards();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка Undo: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Нечего отменять.");
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_undoRedoManager.CanRedo)
            {
                try
                {
                    _undoRedoManager.Redo();
                    LoadSubscriptionCards();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка Redo: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Нечего повторять.");
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
