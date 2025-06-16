using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using WPFK.ViewModels;

namespace WPFK.Views.Users
{
    /// <summary>
    /// Interaction logic for UserDeliveredHistory.xaml
    /// </summary>
    public partial class UserDeliveredHistory : UserControl
    {
        private readonly int userId;

        public UserDeliveredHistory(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            DataContext = new UserDeliveredHistoryViewModel(this.userId);
        }
    }
}