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

     
   
    }
}
