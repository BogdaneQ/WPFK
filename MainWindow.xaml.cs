using System.Collections.Generic;
using System.Windows;
using WPFK.Data;
using WPFK.Models;


namespace WPFK
{
    public partial class MainWindow : Window
    {
        private List<User> _users;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUsers();
        }

        private void InitializeUsers()
        {
            _users = new List<User>
            {
                new User { Username = "admin", Password = "admin123" },
                new User { Username = "user", Password = "password" }
            };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                MessageBox.Show($"Zalogowano jako {user.Username}");
                var dashboard = new DashboardWindow();
                dashboard.Show();
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
