using System.Windows;
using WPFK.Data;
using WPFK.Models;

namespace WPFK
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            var user = new User { Username = username, Password = password };
            db.Users.Add(user);
            db.SaveChanges();

            MessageBox.Show("Rejestracja zakończona!");
            this.Close();
        }
    }
}

