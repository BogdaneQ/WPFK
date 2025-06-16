using System.Collections.Generic;
using System.Windows;
using WPFK.Data;
using WPFK.Helpers;
using WPFK.Models;
using WPFK.Views;
using WPFK.Views.Users;
using WPFK.Views.Admin;
using System.Windows.Controls;
using WPFK.ViewModels;



namespace WPFK
{
    public partial class MainWindow : Window
    {
        private List<User> _users;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WPFK.ViewModels.LoginViewModel();
        }

        //hasło nie jest bindowane
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }



    }
}
