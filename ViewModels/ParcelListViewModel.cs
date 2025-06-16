using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFK.Data;
using WPFK.Helpers;



namespace WPFK.ViewModels
{
    internal class ParcelListViewModel : BaseViewModel
    {
        public ObservableCollection<ParcelViewModel> Parcels { get; } = new();

        public ICommand RefreshCommand { get; }

        public ParcelListViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await LoadParcelsAsync());
        }


        public async Task LoadParcelsAsync()
        {
            Parcels.Clear();

            var userId = Session.CurrentUser?.Id;
            if (userId == null)
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var parcels = db.Parcels
                    .Where(p => p.SenderId == userId || p.RecipientId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new ParcelViewModel
                    {
                        Id = p.Id,
                        SenderName = p.Sender.Username,
                        RecipientName = p.Recipient.Username,
                        Address = p.Address,
                        Status = p.Status,
                        CreatedAt = p.CreatedAt
                    })
                    .ToList();

                foreach (var parcel in parcels)
                    Parcels.Add(parcel);
            }
            catch
            {
                MessageBox.Show("Błąd podczas ładowania paczek.");
            }
        }
    }
}