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
    /// Логика взаимодействия для EditReviewWindow.xaml
    /// </summary>
    public partial class EditReviewWindow : Window
    {
        private readonly Review _review;

        public EditReviewWindow(Review review)
        {
            InitializeComponent();
            _review = review;
            ReviewTextBox.Text = review.Text;
            RatingComboBox.SelectedIndex = review.Rating - 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text))
            {
                MessageBox.Show("Введите текст отзыва");
                return;
            }

            _review.Text = ReviewTextBox.Text;
            _review.Rating = RatingComboBox.SelectedIndex + 1;

            if (DatabaseHelper.UpdateReview(_review.ReviewId, _review.Text, _review.Rating))
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
