using System.Linq;
using System.Windows.Input;
using WPFK.Data;
using WPFK.Helpers;
using WPFK.Models;

namespace WPFK.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand RegisterCommand { get; }

        private string _registrationMessage;
        public string RegistrationMessage
        {
            get => _registrationMessage;
            set
            {
                _registrationMessage = value;
                OnPropertyChanged();
            }
        }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register()
        {
            using var db = new AppDbContext();

            bool userExists = db.Users.Any(u => u.Username == Username);
            if (userExists)
            {
                RegistrationMessage = "Username is already taken.";
                return;
            }
            var user = new User { Username = Username, Password = Password, Role = "USER" };
            db.Users.Add(user);
            db.SaveChanges();
            RegistrationMessage = "Registration successful!";
        }
    }
}