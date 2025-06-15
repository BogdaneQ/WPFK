using System.Collections.Generic;
using System.Windows;
using WPFK.Data;
using WPFK.Helpers;
using WPFK.Models;
using WPFK.Views;
using WPFK.Views.Users;



namespace WPFK
{
    public partial class MainWindow : Window
    {
        private List<User> _users;

        public MainWindow()
        {
            InitializeComponent();
          
        }

        

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {


            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                Session.CurrentUser = user;
                MessageBox.Show($"Zalogowano jako {user.Username}");
                
                if (Session.CurrentUser.Role == "ADMIN")
                {
                    var dashboard = new DashboardWindow();
                    dashboard.Show();
                }
                if (Session.CurrentUser.Role == "USER")
                {
                    var home = new UserHomePage();
                    var homeWindow = new Window
                    {
                        Content = home,
                        Title = "User Home Page",
                        Width = 800,
                        Height = 600
                    };
                    homeWindow.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane logowania.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().ShowDialog();
        }


    }
}
