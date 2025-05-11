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

        public AddReview()
        {
            InitializeComponent();
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем введенный текст
            ReviewText = ReviewTextBox.Text;

            // Закрываем окно, возвращая true
            this.DialogResult = true;
            this.Close();
        }
    }
}
