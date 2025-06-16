using System.Windows;
using WPFK.Helpers;
using WPFK.ViewModels;

namespace WPFK.Views.Admin
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            DataContext = new ParcelListViewModel();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Wyloguj użytkownika (np. wyczyść sesję)
            Session.CurrentUser = null;

            // Zamknij to okno i pokaż okno logowania
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}