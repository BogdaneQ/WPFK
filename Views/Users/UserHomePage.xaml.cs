using System.Windows.Controls;
using WPFK.Data;
using System.Linq;
using WPFK.Helpers;
using System.Windows;
using WPFK.ViewModels;

namespace WPFK.Views.Users
{
    public partial class UserHomePage : UserControl
    {
        public UserHomePage()
        {
            InitializeComponent();
            var vm = new ParcelListViewModel();
            DataContext = vm;
            _ = vm.LoadParcelsAsync();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            
            Session.CurrentUser = null;

            
            var loginWindow = new MainWindow();
            Window.GetWindow(this)?.Close();
            loginWindow.Show();
        }
        private void ShowDeliveredHistory_Click(object sender, RoutedEventArgs e)
        {
            var userId = Session.CurrentUser?.Id ?? 0;
            var historyView = new WPFK.Views.Users.UserDeliveredHistory(userId); // Pass userId to the constructor

            // Set the DataContext on the Window, not the Page
            var window = new Window
            {
                Title = "Historia dostarczonych przesyłek",
                Content = historyView,
                Width = 600,
                Height = 400,
                DataContext = new WPFK.ViewModels.UserDeliveredHistoryViewModel(userId)
            };
            window.ShowDialog();
        }



    }
}
