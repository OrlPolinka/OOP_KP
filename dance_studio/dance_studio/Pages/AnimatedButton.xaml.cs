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
    /// Логика взаимодействия для AnimatedButton.xaml
    /// </summary>
    public partial class AnimatedButton : UserControl
    {
        public AnimatedButton()
        {
            InitializeComponent();
        }
        public void GoToSignUp(object sender, RoutedEventArgs e)
        {
            if (Seccion.IsClient)
            {
                var navService = NavigationService.GetNavigationService(this);
                if (navService != null)
                {
                    navService.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("NavigationService не найден");
                }
            }
            else { MessageBox.Show("Страница записи только для пользователей"); }
        }
    }
}
