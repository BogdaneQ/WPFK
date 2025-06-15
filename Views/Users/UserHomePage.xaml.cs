using System.Windows.Controls;
using WPFK.Data;
using System.Linq;
using WPFK.Helpers;
using System.Windows;

namespace WPFK.Views.Users

{
    public partial class UserHomePage: UserControl
    {
        public UserHomePage()
        {
           
            LoadParcels();
        }

        private void LoadParcels()
        {
            using var db = new AppDbContext();
            // Załóżmy, że masz globalnie dostępną sesję z użytkownikiem
            var userId = Session.CurrentUser?.Id;

            if (userId == null)
            {
                //ParcelListView.ItemsSource = null;
                return;
            }

            // Załaduj tylko przesyłki przypisane do aktualnego użytkownika
            var parcels = db.Parcels
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            //ParcelListView.ItemsSource = parcels;
        }

        private void RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadParcels();
        }
    }
}
