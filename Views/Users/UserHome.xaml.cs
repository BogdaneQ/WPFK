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
using System.Windows.Shapes;
using WPFK.Data;
using WPFK.Helpers;
using WPFK.Models;

namespace WPFK.Views
{
    /// <summary>
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Window
    {
        public UserHome()
        {
            InitializeComponent();
            LoadUserParcels();
        }

        private void LoadUserParcels()
        {
            using var db = new AppDbContext();
            var userId = Session.CurrentUser.Id;

            ParcelListView.ItemsSource = db.Parcels
                .Where(p => p.UserId == userId)
                .ToList();
        }


        private void RefreshParcels_Click(object sender, RoutedEventArgs e)
        {
            LoadUserParcels();
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelListView.SelectedItem is Parcel selectedParcel)
            {
                var window = new StatusHistoryWindow(selectedParcel.Id);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz przesyłkę.");
            }
        }
    }
}
