using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using dance_studio;
using System.Security.Policy;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace dance_studio
{
    public class Client_records
    {
        public TimeSpan Time { get; set; }
        public string DayOfTheWeek { get; set; }
        public string Trainers { get; set; }
        public string Style { get; set; }
        public string Levels { get; set; }
    }
    public class Styles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionEn { get; set; }

        // Свойства, которые возвращают данные в зависимости от языка
        public string CurrentTitle => Lang.currentLanguageCode == "en" ? TitleEn : Title;
        public string CurrentDescription => Lang.currentLanguageCode == "en" ? DescriptionEn : Description;
    }

    public partial class Trainers
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Fio { get; set; }
        public string Bio { get; set; }
        public string FioEn { get; set; }
        public string BioEn { get; set; }
        // Свойства, которые возвращают данные в зависимости от языка
        public string CurrentFio => Lang.currentLanguageCode == "en" ? FioEn : Fio;
        public string CurrentBio => Lang.currentLanguageCode == "en" ? BioEn : Bio;
    }



    public class Abonements
    {
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string Style { get; set; }
        public string StyleEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string Price { get; set; }
        // Динамические свойства для текущего языка
        public string CurrentTitle => Lang.currentLanguageCode == "en" ? TitleEn : Title;
        public string CurrentStyle => Lang.currentLanguageCode == "en" ? StyleEn : Style;
        public string CurrentDescription => Lang.currentLanguageCode == "en" ? DescriptionEn : Description;
    }

    public class Subscription
    {
        public string SubsTitle { get; set; }
        public string Phone { get; set; }
        public string Price { get; set; }
        public string Inst { get; set; }
        public string Buys { get; set; }
    }


    // Класс модели для новостей
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }


    }

    public class Seccion
    {
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static bool IsClient => Role == "Клиент";
    }


    public class TimetableEntry
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public string Time { get; set; }
        public string Style { get; set; } // Название стиля
        public string Trainer { get; set; } // ФИО тренера
    }

    internal class DatabaseHelper
    {

        public static void AddNotifications(string promotionTitle)
        {
            string query = @"
        UPDATE CLIENTS
        SET NOTIFICATIONS = 
            CASE 
                WHEN NOTIFICATIONS IS NULL OR NOTIFICATIONS = '' 
                THEN CONCAT('Добавлена новая акция: ', @promotionTitle)
                ELSE CONCAT(NOTIFICATIONS, '; ', 'Добавлена новая акция: ', @promotionTitle)
            END";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@promotionTitle", promotionTitle);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Debug.WriteLine($"Уведомления добавлены для {rowsAffected} клиентов");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при добавлении уведомлений: {ex.Message}");
                    throw; // Можно заменить на MessageBox.Show() в UI-приложении
                }
            }
        }

        public static bool AddStyle(string title, string description, string titleEn, string descriptionEn)
        {
            string query = @"
        INSERT INTO STYLES 
            (STYLE_TITLE, STYLE_DESCRRIPTION, STYLE_TITLE_EN, STYLE_DESCRIPTION_EN) 
        VALUES 
            (@Title, @Description, @TitleEn, @DescriptionEn)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@TitleEn", titleEn);
                command.Parameters.AddWithValue("@DescriptionEn", descriptionEn);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении стиля: " + ex.Message);
                    return false;
                }
            }
        }

        public static bool DeleteStyleFromDatabase(int styleId)
        {
            try
            {
                string query = "DELETE FROM STYLES WHERE STYLE_ID = @StyleId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StyleId", styleId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                return false;
            }
        }

        public static List<Styles> GetStyles()
        {
            List<Styles> stylesList = new List<Styles>();
            string query = "SELECT * FROM STYLES";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    stylesList.Add(new Styles
                    {
                        Id = Convert.ToInt32(reader["STYLE_ID"]),
                        Title = reader["STYLE_TITLE"]?.ToString() ?? string.Empty,
                        Description = reader["STYLE_DESCRRIPTION"]?.ToString() ?? string.Empty,
                        TitleEn = reader["STYLE_TITLE_EN"]?.ToString() ?? string.Empty,
                        DescriptionEn = reader["STYLE_DESCRIPTION_EN"]?.ToString() ?? string.Empty
                    });
                }
            }
            return stylesList;
        }


        public static List<TimetableEntry> GetTimetable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT t.ID, t.DAY_OF_WEEK, t.TIME, 
                   s.STYLE_TITLE AS STYLE, 
                   tr.FIO AS TRAINER
            FROM TIMETABLE t
            JOIN STYLES s ON t.STYLE_ID = s.STYLE_ID
            JOIN TRAINERS tr ON t.TRAINER_ID = tr.TRAINERS_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<TimetableEntry> list = new List<TimetableEntry>();
                while (reader.Read())
                {
                    list.Add(new TimetableEntry
                    {
                        Id = (int)reader["ID"],
                        DayOfWeek = reader["DAY_OF_WEEK"].ToString(),
                        Time = reader["TIME"].ToString(),
                        Style = reader["STYLE"].ToString(),
                        Trainer = reader["TRAINER"].ToString()
                    });
                }
                return list;
            }
        }




        public static void UpdateTimetable(TimetableEntry entry)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int styleId = GetStyleIdByTitle(conn, entry.Style);
                int trainerId = GetTrainerIdByFIO(conn, entry.Trainer);

                string query = @"
            UPDATE TIMETABLE 
            SET DAY_OF_WEEK = @DayOfWeek, TIME = @Time, 
                STYLE_ID = @StyleId, TRAINER_ID = @TrainerId
            WHERE ID = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", entry.Id);
                cmd.Parameters.AddWithValue("@DayOfWeek", entry.DayOfWeek);
                cmd.Parameters.AddWithValue("@Time", entry.Time);
                cmd.Parameters.AddWithValue("@StyleId", styleId);
                cmd.Parameters.AddWithValue("@TrainerId", trainerId);

                cmd.ExecuteNonQuery();
            }
        }


        public static void DeleteTimetable(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM TIMETABLE WHERE ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }


        public static void AddTimetable(TimetableEntry entry)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int styleId = GetStyleIdByTitle(conn, entry.Style);
                int trainerId = GetTrainerIdByFIO(conn, entry.Trainer);

                string query = @"
            INSERT INTO TIMETABLE (DAY_OF_WEEK, TIME, STYLE_ID, TRAINER_ID)
            VALUES (@DayOfWeek, @Time, @StyleId, @TrainerId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DayOfWeek", entry.DayOfWeek);
                cmd.Parameters.AddWithValue("@Time", entry.Time);
                cmd.Parameters.AddWithValue("@StyleId", styleId);
                cmd.Parameters.AddWithValue("@TrainerId", trainerId);

                cmd.ExecuteNonQuery();
            }
        }

        private static int GetStyleIdByTitle(SqlConnection conn, string styleTitle)
        {
            SqlCommand cmd = new SqlCommand("SELECT STYLE_ID FROM STYLES WHERE STYLE_TITLE = @title", conn);
            cmd.Parameters.AddWithValue("@title", styleTitle);
            object result = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception($"Стиль '{styleTitle}' не найден.");
            return (int)result;
        }

        private static int GetTrainerIdByFIO(SqlConnection conn, string trainerFIO)
        {
            SqlCommand cmd = new SqlCommand("SELECT TRAINERS_ID FROM TRAINERS WHERE FIO = @fio", conn);
            cmd.Parameters.AddWithValue("@fio", trainerFIO);
            object result = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception($"Тренер '{trainerFIO}' не найден.");
            return (int)result;
        }



        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DANCE_STUDIO_BD"].ConnectionString;

        public static bool CheckUserExists(string username, string password, string role)
        {
            string table = role == "Администратор" ? "ADMINS" : "CLIENTS";

            string query = $"SELECT COUNT(*) FROM {table} WHERE USER_NAME = @username AND PASSWORD = @password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();

                // Получаем количество записей, которые соответствуют запросу
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }


        public static bool RegisterUser(string username, string password, string role)
        {
            string table = role == "Администратор" ? "ADMINS" : "CLIENTS";
            string query = $"INSERT INTO {table} (USER_NAME, PASSWORD) VALUES (@username, @password)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException)
                {
                    return false; // например, если логин уже существует
                }
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes); // хешируем пароль
            }
        }


        public static List<Abonements> GetAbonements()
        {
            var abonements = new List<Abonements>();
            string query = "SELECT TITLE, TITLE_EN, PRICE, STYLE, STYLE_EN FROM ABONEMENTS";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        abonements.Add(new Abonements
                        {
                            Title = reader["TITLE"].ToString(),
                            TitleEn = reader["TITLE_EN"]?.ToString() ?? string.Empty,
                            Style = reader["STYLE"]?.ToString() ?? string.Empty,
                            StyleEn = reader["STYLE_EN"]?.ToString() ?? string.Empty,
                            Price = reader["PRICE"].ToString()
                        });
                    }
                }
            }

            return abonements;
        }

        public static Abonements GetAbonement(string title)
        {
            Abonements abonement = null;
            string query = "SELECT*FROM ABONEMENTS WHERE TITLE = @title OR TITLE_EN = @title";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))

                {
                    cmd.Parameters.AddWithValue("@title", title);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            abonement = new Abonements
                            {
                                Title = reader["TITLE"].ToString(),
                                TitleEn = reader["TITLE_EN"]?.ToString() ?? string.Empty,
                                Style = reader["STYLE"]?.ToString() ?? string.Empty,
                                StyleEn = reader["STYLE_EN"]?.ToString() ?? string.Empty,
                                Description = reader["DESCRIPTION"]?.ToString() ?? string.Empty,
                                DescriptionEn = reader["DESCRIPTION_EN"]?.ToString() ?? string.Empty,
                                Price = reader["PRICE"].ToString()
                            };
                        }
                    }
                }

                return abonement;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении абонемента: " + ex.Message);
                return null;
            }
        }   


        public static bool DeleteAbonementByTitle(string title)
        {
            string query = "DELETE FROM ABONEMENTS WHERE TITLE = @Title";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Title", title);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


        public static bool AddAbonement(string title, string description, string price, string style,
                                string titleEn, string descriptionEn, string styleEn)
        {
            // Обновленный запрос с добавлением английских версий
            string query = "INSERT INTO ABONEMENTS (TITLE, DESCRIPTION, PRICE, STYLE, " +
                          "TITLE_EN, DESCRIPTION_EN, STYLE_EN) " +
                          "VALUES (@title, @description, @price, @style, " +
                          "@titleEn, @descriptionEn, @styleEn)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Параметры для русской версии
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description",
                        string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@style",
                        string.IsNullOrEmpty(style) ? (object)DBNull.Value : style);

                    // Параметры для английской версии
                    cmd.Parameters.AddWithValue("@titleEn",
                        string.IsNullOrEmpty(titleEn) ? (object)DBNull.Value : titleEn);
                    cmd.Parameters.AddWithValue("@descriptionEn",
                        string.IsNullOrEmpty(descriptionEn) ? (object)DBNull.Value : descriptionEn);
                    cmd.Parameters.AddWithValue("@styleEn",
                        string.IsNullOrEmpty(styleEn) ? (object)DBNull.Value : styleEn);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении абонемента: " + ex.Message);
                return false;
            }
        }



        public static List<Subscription> GetUserSubscriptions(string username)
        {
            List<Subscription> subscriptions = new List<Subscription>();

            string query = $"SELECT SUBS_TITLE, PRICE, BUYS FROM CLIENT_SUBSCRIPTIONS WHERE USER_NAME = @username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Subscription sub = new Subscription
                        {
                            SubsTitle = reader["SUBS_TITLE"].ToString(),
                            Price = reader["PRICE"].ToString(),
                            Buys = reader["BUYS"].ToString()
                        };
                        subscriptions.Add(sub);
                    }
                }
            }

            return subscriptions;
        }

        public static List<Client_records> LoadClientRecords(string username)
        {
            List<Client_records> records = new List<Client_records>();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                string query = @"
            SELECT 
                CR.TIME,
                CR.DAY_OF_WEEK,
                T.FIO AS TrainerName,
                S.STYLE_TITLE AS StyleTitle,
                CR.LEVELS
            FROM CLIENT_RECORDS CR
            JOIN TRAINERS T ON CR.TRAINERS_ID = T.TRAINERS_ID
            JOIN STYLES S ON CR.STYLE_ID = S.STYLE_ID
            WHERE CR.USER_NAME = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", Seccion.Username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new Client_records
                            {
                                Time = TimeSpan.Parse(reader["TIME"].ToString()),
                                DayOfTheWeek = reader["DAY_OF_WEEK"].ToString(),
                                Trainers = reader["TrainerName"].ToString(),
                                Style = reader["StyleTitle"].ToString(),
                                Levels = reader["LEVELS"].ToString()
                            };
                            records.Add(record);
                        }
                    }
                }
                return records;
            }
        }


        public static bool UpdateUsernameInDatabase(string oldUsername, string newUsername, string password, string phone, string email)
        {
            try
            {
                string query = "UPDATE CLIENTS SET USER_NAME = @newUsername, PASSWORD = @password, PHONE = @phone, EMAIL = @email WHERE USER_NAME = @oldUsername";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newUsername", newUsername);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@oldUsername", oldUsername);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении имени в базе данных: " + ex.Message);
                return false;
            }
        }


        public static List<News> GetNewsFromDatabase()
        {
            List<News> newsList = new List<News>();
            string languageCode = Lang.currentLanguageCode;

            string query = @"
SELECT N.ID, NL.TITLE, N.PUBLISH_DATE, NL.DESCRIPTION, N.IMAGEPATH
FROM NEWS N
INNER JOIN NEWSLOCALIZATION NL ON N.ID = NL.NewsID
WHERE NL.LanguageCode = @LanguageCode
ORDER BY N.PUBLISH_DATE DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LanguageCode", languageCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    newsList.Add(new News
                    {
                        Id = (int)reader["ID"],
                        Title = reader["TITLE"].ToString(),
                        PublishDate = (DateTime)reader["PUBLISH_DATE"],
                        Description = reader["DESCRIPTION"].ToString(),
                        ImagePath = reader["IMAGEPATH"].ToString()
                    });
                }
            }
            return newsList;
        }


        public static bool SaveNewsToDatabaseWithLocalization(string titleRu, string titleEn, string descRu, string descEn, DateTime publishDate, string imagePath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    // Сохраняем основную новость
                    string query = "INSERT INTO NEWS (PUBLISH_DATE, IMAGEPATH) VALUES (@PublishDate, @ImagePath); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, conn, transaction);
                    cmd.Parameters.AddWithValue("@PublishDate", publishDate);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);

                    int newsId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Сохраняем локализованные данные
                    string localizationQuery = "INSERT INTO NEWSLOCALIZATION (NewsID, LanguageCode, Title, Description) VALUES (@NewsID, @LanguageCode, @Title, @Description)";

                    cmd = new SqlCommand(localizationQuery, conn, transaction);

                    // Для русского языка
                    cmd.Parameters.AddWithValue("@NewsID", newsId);
                    cmd.Parameters.AddWithValue("@LanguageCode", "ru");
                    cmd.Parameters.AddWithValue("@Title", titleRu);
                    cmd.Parameters.AddWithValue("@Description", descRu);
                    cmd.ExecuteNonQuery();

                    // Для английского языка
                    cmd.Parameters["@LanguageCode"].Value = "en";
                    cmd.Parameters["@Title"].Value = titleEn;
                    cmd.Parameters["@Description"].Value = descEn;
                    cmd.ExecuteNonQuery();

                    // Завершаем транзакцию
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Удаление новости из базы данных
        public static bool DeleteNewsFromDatabase(int newsId)
        {
            try
            {
                string query = "DELETE FROM NEWS WHERE ID = @NewsId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NewsId", newsId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static void AddReviewToDatabase(string reviewText, string userName)
        {

            string query = "INSERT INTO Reviews (REV_TEXT, USER_NAME) VALUES (@ReviewText, @UserName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ReviewText", reviewText);
                command.Parameters.AddWithValue("@UserName", userName); // Добавляем имя пользователя

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении отзыва: " + ex.Message);
                }
            }
        }



        public static List<Trainers> GetTrainers()
        {
            List<Trainers> trainersList = new List<Trainers>();
            string query = "SELECT * FROM TRAINERS";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    trainersList.Add(new Trainers
                    {
                        Id = Convert.ToInt32(reader["TRAINERS_ID"]),
                        Fio = reader["FIO"].ToString(),
                        Bio = reader["BIO"].ToString(),
                        ImagePath = reader["IMAGEPATH"].ToString(),
                        FioEn = reader["FIO_EN"].ToString(),
                        BioEn = reader["BIO_EN"].ToString(),
                    });
                }
            }
            return trainersList;
        }

        public static bool AddTrainersToDB(string fio, string bio, string imagePath, string fioEn, string bioEn)
        {
            string query = "INSERT INTO TRAINERS (IMAGEPATH, FIO, BIO, FIO_EN, BIO_EN) VALUES (@ImagePath, @FIO, @BIO, @FIO_EN, @BIO_EN)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ImagePath", imagePath);
                command.Parameters.AddWithValue("@FIO", fio);
                command.Parameters.AddWithValue("@BIO", string.IsNullOrEmpty(bio) ? (object)DBNull.Value : bio);

                // Параметры для английской версии
                command.Parameters.AddWithValue("@FIO_EN", string.IsNullOrEmpty(fioEn) ? (object)DBNull.Value : fioEn);
                command.Parameters.AddWithValue("@BIO_EN", string.IsNullOrEmpty(bioEn) ? (object)DBNull.Value : bioEn);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении отзыва: " + ex.Message);
                    return false;
                }
            }
        }

        public static bool DeleteTrainersFromDatabase(int trainerId)
        {
            try
            {
                string query = "DELETE FROM TRAINERS WHERE TRAINERS_ID = @TrainerId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainerId", trainerId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                return false;
            }
        }
    }
}