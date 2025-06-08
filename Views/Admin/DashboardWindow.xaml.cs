using System.Windows;
using WPFK.Data;
using WPFK.Models;
using System.Linq;
using System.Windows.Controls;
using WPFK.Helpers;

namespace WPFK
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            LoadParcels();
            StatusComboBox.SelectedIndex = 0;
        }

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();

            var status = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Content.ToString();

            var parcel = new Parcel
            {
                Sender = SenderTextBox.Text,
                Recipient = RecipientTextBox.Text,
                Address = AddressTextBox.Text,
                Status = status,
                CreatedAt = DateTime.UtcNow, // <--- użyj UTC
                UserId = Session.CurrentUser.Id
            };

            db.Parcels.Add(parcel);


            db.SaveChanges();

            db.ParcelStatusHistories.Add(new ParcelStatusHistory
            {
                ParcelId = parcel.Id,
                Status = status,
                ChangedAt = DateTime.UtcNow // <--- również tutaj użyj UTC
            });

            db.SaveChanges();

            MessageBox.Show("Przesyłka dodana!");
            LoadParcels();
        }


        private void FilterParcels_Click(object sender, RoutedEventArgs e)
        {
            string selectedStatus = ((ComboBoxItem)FilterStatusComboBox.SelectedItem)?.Content.ToString();
            using var db = new AppDbContext();

            if (selectedStatus == "Wszystkie")
            {
                ParcelListView.ItemsSource = db.Parcels.ToList();
            }
            else
            {
                ParcelListView.ItemsSource = db.Parcels
                    .Where(p => p.Status == selectedStatus)
                    .ToList();
            }
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

        private void EditParcel_Click(object sender, RoutedEventArgs e)
        {
            if (selectedParcel == null)
            {
                MessageBox.Show("Wybierz przesyłkę do edycji.");
                return;
            }

            using var db = new AppDbContext();
            var parcelToUpdate = db.Parcels.FirstOrDefault(p => p.Id == selectedParcel.Id);

            if (parcelToUpdate != null)
            {
                parcelToUpdate.Sender = SenderTextBox.Text;
                parcelToUpdate.Recipient = RecipientTextBox.Text;
                parcelToUpdate.Address = AddressTextBox.Text;

                var newStatus = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Content.ToString();

                if (parcelToUpdate.Status != newStatus)
                {
                    parcelToUpdate.Status = newStatus;

                    // Dodaj wpis do historii zmian
                    db.ParcelStatusHistories.Add(new ParcelStatusHistory
                    {
                        ParcelId = parcelToUpdate.Id,
                        Status = newStatus,
                        ChangedAt = DateTime.UtcNow
                    });
                }

                db.SaveChanges();
                MessageBox.Show("Przesyłka zaktualizowana.");
                LoadParcels();
            }

            selectedParcel = null;
            SenderTextBox.Text = "";
            RecipientTextBox.Text = "";
            AddressTextBox.Text = "";
            StatusComboBox.SelectedIndex = 0;
        }



        private void LoadParcels()
        {
            using var db = new AppDbContext();
            ParcelListView.ItemsSource = db.Parcels.ToList();
        }

        private void RefreshParcels_Click(object sender, RoutedEventArgs e)
        {
            LoadParcels();
        }

        private Parcel selectedParcel;
        private void ParcelListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedParcel = ParcelListView.SelectedItem as Parcel;

            if (selectedParcel != null)
            {
                SenderTextBox.Text = selectedParcel.Sender;
                RecipientTextBox.Text = selectedParcel.Recipient;
                AddressTextBox.Text = selectedParcel.Address;

                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if ((string)item.Content == selectedParcel.Status)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }
    }
}

