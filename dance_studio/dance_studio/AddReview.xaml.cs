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
using System.Windows.Shapes;

namespace dance_studio
{
    /// <summary>
    /// Логика взаимодействия для AddReview.xaml
    /// </summary>
    public partial class AddReview : Window
    {
        public string ReviewText { get; private set; }
        public int Rating { get; private set; }

        public AddReview()
        {
            InitializeComponent();
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text))
            {
                MessageBox.Show("Введите текст отзыва");
                return;
            }

            ReviewText = ReviewTextBox.Text;
            Rating = RatingComboBox.SelectedIndex + 1; // Индексы начинаются с 0, поэтому +1

            this.DialogResult = true;
            this.Close();
        }
    }
}
