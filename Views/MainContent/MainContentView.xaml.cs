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
using System.Windows.Shapes;
using WPFK.Views.Users;

namespace WPFK.Views.MainContent
{
    /// <summary>
    /// Logika interakcji dla klasy MainContentView.xaml
    /// </summary>
    public partial class MainContentView : Window
    {
        public MainContentView()
        {
            InitializeComponent();
            NavigateTo(new UserHomePage()); // Startowy widok
        }

        public void NavigateTo(UserControl newPage)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            fadeOut.Completed += (s, e) =>
            {
                MainContent.Content = newPage;

                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
                MainContent.BeginAnimation(OpacityProperty, fadeIn);
            };

            MainContent.BeginAnimation(OpacityProperty, fadeOut);
        }
    }

}
