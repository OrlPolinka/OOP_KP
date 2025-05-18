using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private bool isLoggedInAsClient; // флаг, получаем из логики входа

        public SignUp()
        {
            try
            {
                InitializeComponent();
                AbonementComboBox.SelectionChanged += AbonementComboBox_SelectionChanged;
                this.Loaded += Data_Loaded;
                //PhoneBox1.PhoneTextBox.Text = "+375 (29) 123-45-67";
                //PhoneBox2.PhoneTextBox.Text = "+375 (29) 123-45-67";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void PhoneBox1_PhoneValidated(object sender, RoutedEventArgs e)
        {
            string phone = PhoneBox1.PhoneTextBox.Text;
            MessageBox.Show($"Номер телефона валиден: {phone}");
        }

        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
                if (!string.IsNullOrEmpty(Seccion.Username))
                {
                    // Если у вас TextBox для имени пользователя
                    UserComboBox1.Text = Seccion.Username;
                    UserComboBox2.Text = Seccion.Username;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }


        public void AbonementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = AbonementComboBox.SelectedItem as DataRowView;
            if (selected != null)
            {
                PriceTextBox.Text = selected["PRICE"].ToString();
            }
        }


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DANCE_STUDIO_BD"].ConnectionString;

        public void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Пользователи
                    SqlDataAdapter userAdapter = new SqlDataAdapter("SELECT USER_NAME FROM CLIENTS", conn);
                    DataTable userTable = new DataTable();
                    userAdapter.Fill(userTable);
                    //UserComboBox1.Text = userTable.DefaultView;
                    //UserComboBox2.Text = userTable.DefaultView;

                    // Абонементы
                    SqlDataAdapter abAdapter = new SqlDataAdapter("SELECT PRICE, TITLE FROM ABONEMENTS", conn);
                    DataTable abTable = new DataTable();
                    abAdapter.Fill(abTable);
                    AbonementComboBox.ItemsSource = abTable.DefaultView;
                    AbonementComboBox.DisplayMemberPath = "TITLE";
                    // Внутренне используется вся строка DataRowView
                    AbonementComboBox.SelectedIndex = 0;


                    // Загрузка уникальных времён
                    SqlDataAdapter timeAdapter = new SqlDataAdapter("SELECT DISTINCT TIME FROM TIMETABLE", conn);
                    DataTable timeTable = new DataTable();
                    timeAdapter.Fill(timeTable);
                    TimeComboBox.ItemsSource = timeTable.DefaultView;

                    
                    SqlDataAdapter dayOfWeekAdapter = new SqlDataAdapter("SELECT DISTINCT DAY_OF_WEEK FROM TIMETABLE", conn);
                    DataTable dayOfWeekTable = new DataTable();
                    dayOfWeekAdapter.Fill(dayOfWeekTable);
                    DayOfWeekComboBox.DisplayMemberPath = "DAY_OF_WEEK";
                    DayOfWeekComboBox.ItemsSource = dayOfWeekTable.DefaultView;

                    // Загрузка уникальных стилей из связанной таблицы
                    SqlDataAdapter styleAdapter = new SqlDataAdapter(@"
    SELECT DISTINCT S.STYLE_TITLE 
    FROM TIMETABLE T
    JOIN STYLES S ON T.STYLE_ID = S.STYLE_ID", conn);
                    DataTable styleTable = new DataTable();
                    styleAdapter.Fill(styleTable);
                    StyleComboBox.ItemsSource = styleTable.DefaultView;

                    // Загрузка уникальных тренеров из связанной таблицы
                    SqlDataAdapter trainerAdapter = new SqlDataAdapter(@"
    SELECT DISTINCT T.FIO 
    FROM TRAINERS T
    JOIN TIMETABLE TR ON T.TRAINERS_ID = TR.TRAINER_ID", conn);
                    DataTable trainerTable = new DataTable();
                    trainerAdapter.Fill(trainerTable);
                    TrainerComboBox.ItemsSource = trainerTable.DefaultView;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void BuyAbonement_Click(object sender, RoutedEventArgs e)
        {
            string username = UserComboBox1.Text;
            string abonement = (AbonementComboBox.SelectedItem as DataRowView)?["TITLE"].ToString();
            string phone = PhoneBox1.Text;
            string insta = InstagramBox1.Text;
            string priceText = PriceTextBox.Text;
            string payment = (PaymentBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (username == null || abonement == null  || payment == null || phone == null || insta == null || priceText == null) { MessageBox.Show("Заполните все поля"); return; }

            decimal price;
            if (!decimal.TryParse(priceText, out price))
            {
                MessageBox.Show("Некорректная цена.");
                return;
            }


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO CLIENT_SUBSCRIPTIONS (USER_NAME, SUBS_TITLE, PHONE, INST, PRICE, BUYS)
                         VALUES (@USER_NAME, @SUBS_TITLE, @PHONE, @INST, @PRICE, @BUYS)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@USER_NAME", username);
                    cmd.Parameters.AddWithValue("@SUBS_TITLE", abonement);
                    cmd.Parameters.AddWithValue("@PHONE", phone);
                    cmd.Parameters.AddWithValue("@INST", insta);
                    cmd.Parameters.AddWithValue("@PRICE", priceText);
                    cmd.Parameters.AddWithValue("@BUYS", payment);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Абонемент успешно куплен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении абонемента: {ex.Message}");
            }
        }



        public void Register_Click(object sender, RoutedEventArgs e)
        {


            if (UserComboBox2.Text == null ||
    TimeComboBox.SelectedItem == null ||
    TrainerComboBox.SelectedItem == null ||
    StyleComboBox.SelectedItem == null ||
    DayOfWeekComboBox == null ||
    LevelBox == null ||
    PhoneBox2 == null ||
    InstagramBox2 == null)
            {
                MessageBox.Show("Выберите все поля из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string username = UserComboBox2.Text;
            string timeString = (TimeComboBox.SelectedItem as DataRowView)?["TIME"].ToString();
            string dayOfTheWeek = (DayOfWeekComboBox.SelectedItem as DataRowView)?["DAY_OF_WEEK"].ToString();
            string trainerName = (TrainerComboBox.SelectedItem as DataRowView)?["FIO"].ToString();
            string styleTitle = (StyleComboBox.SelectedItem as DataRowView)?["STYLE_TITLE"].ToString();
            string phone = PhoneBox2.Text;
            string instagram = InstagramBox2.Text;
            string level = (LevelBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Получение ID тренера по имени
                    string trainerIdQuery = "SELECT TRAINERS_ID FROM TRAINERS WHERE FIO = @TrainerName";
                    SqlCommand trainerCmd = new SqlCommand(trainerIdQuery, conn);
                    trainerCmd.Parameters.AddWithValue("@TrainerName", trainerName);
                    object trainerIdObj = trainerCmd.ExecuteScalar();
                    if (trainerIdObj == null)
                    {
                        MessageBox.Show("Тренер не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    int trainerId = Convert.ToInt32(trainerIdObj);

                    // Получение ID стиля по названию
                    string styleIdQuery = "SELECT STYLE_ID FROM STYLES WHERE STYLE_TITLE = @StyleTitle";
                    SqlCommand styleCmd = new SqlCommand(styleIdQuery, conn);
                    styleCmd.Parameters.AddWithValue("@StyleTitle", styleTitle);
                    object styleIdObj = styleCmd.ExecuteScalar();
                    if (styleIdObj == null)
                    {
                        MessageBox.Show("Стиль не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    int styleId = Convert.ToInt32(styleIdObj);

                    // Добавление записи в CLIENT_RECORDS
                    string insertQuery = @"
                INSERT INTO CLIENT_RECORDS 
                (USER_NAME, TIME, DAY_OF_WEEK, TRAINERS_ID, STYLE_ID, PHONE, INST, LEVELS)
                VALUES 
                (@UserName, @Time, @DayOfTheWeek, @TrainerId, @StyleId, @Phone, @Inst, @Levels)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@UserName", username);
                    insertCmd.Parameters.AddWithValue("@Time", timeString);
                    insertCmd.Parameters.AddWithValue("@DayOfTheWeek", dayOfTheWeek);
                    insertCmd.Parameters.AddWithValue("@TrainerId", trainerId);
                    insertCmd.Parameters.AddWithValue("@StyleId", styleId);
                    insertCmd.Parameters.AddWithValue("@Phone", phone);
                    insertCmd.Parameters.AddWithValue("@Inst", instagram);
                    insertCmd.Parameters.AddWithValue("@Levels", level ?? (object)DBNull.Value);

                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Запись на занятие успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
