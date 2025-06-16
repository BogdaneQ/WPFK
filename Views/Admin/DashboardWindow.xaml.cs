using System.Windows;
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
    }
}