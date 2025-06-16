using System.Windows;
using System.Windows.Input;
using WPFK.Data;
using WPFK.Helpers;
using WPFK.Models;
using WPFK.Views.Admin;
using WPFK.Views.Users;

namespace WPFK.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Login()
        {
            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                Session.CurrentUser = user;
                MessageBox.Show($"Zalogowano jako {user.Username}");

                if (user.Role == "ADMIN")
                {
                    var dashboard = new DashboardWindow();
                    dashboard.Show();
                }
                else if (user.Role == "USER")
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

                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane logowania.");
            }
        }

        private void Register()
        {
            new RegisterWindow().ShowDialog();
        }
    }
}