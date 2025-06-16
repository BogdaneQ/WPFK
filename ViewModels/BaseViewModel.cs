using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using WPFK.Helpers;

namespace WPFK.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                {
                    _logoutCommand = new RelayCommand(Logout);
                }
                return _logoutCommand;
            }
        }

        private void Logout()
        {
            Session.CurrentUser = null;
            var loginWindow = new MainWindow();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsActive)
                {
                    window.Close();
                    break;
                }
            }
            loginWindow.Show();
        }
    }
}