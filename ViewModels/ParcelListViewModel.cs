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

        private string _newParcelSender;
        public string NewParcelSender
        {
            get => _newParcelSender;
            set { _newParcelSender = value; OnPropertyChanged(); }
        }

        private string _newParcelRecipient;
        public string NewParcelRecipient
        {
            get => _newParcelRecipient;
            set { _newParcelRecipient = value; OnPropertyChanged(); }
        }

        private string _newParcelAddress;
        public string NewParcelAddress
        {
            get => _newParcelAddress;
            set { _newParcelAddress = value; OnPropertyChanged(); }
        }

        private string _newParcelStatus;
        public string NewParcelStatus
        {
            get => _newParcelStatus;
            set { _newParcelStatus = value; OnPropertyChanged(); }
        }

        private ParcelViewModel _selectedParcel;
        public ParcelViewModel SelectedParcel
        {
            get => _selectedParcel;
            set { _selectedParcel = value; OnPropertyChanged(); }
        }

        private string _filterStatus;
        public string FilterStatus
        {
            get => _filterStatus;
            set { _filterStatus = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddParcelCommand { get; }
        public ICommand EditParcelCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ShowHistoryCommand { get; }

        public ParcelListViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await LoadParcelsAsync());
            AddParcelCommand = new RelayCommand(AddParcel);
            EditParcelCommand = new RelayCommand(EditParcel, () => SelectedParcel != null);
            FilterCommand = new RelayCommand(FilterParcels);
            ShowHistoryCommand = new RelayCommand(ShowHistory);
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
                var query = db.Parcels
                    .Where(p => p.SenderId == userId || p.RecipientId == userId);

                if (!string.IsNullOrEmpty(FilterStatus) && FilterStatus != "Wszystkie")
                {
                    query = query.Where(p => p.Status == FilterStatus);
                }

                var parcels = query
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

        private void AddParcel()
        {
            if (string.IsNullOrWhiteSpace(NewParcelSender) ||
                string.IsNullOrWhiteSpace(NewParcelRecipient) ||
                string.IsNullOrWhiteSpace(NewParcelAddress) ||
                string.IsNullOrWhiteSpace(NewParcelStatus))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.");
                return;
            }

            try
            {
                using var db = new AppDbContext();

                var sender = db.Users.FirstOrDefault(u => u.Username == NewParcelSender);
                var recipient = db.Users.FirstOrDefault(u => u.Username == NewParcelRecipient);

                if (sender == null || recipient == null)
                {
                    MessageBox.Show("Nie znaleziono nadawcy lub odbiorcy.");
                    return;
                }

                var parcel = new Models.Parcel
                {
                    SenderId = sender.Id,
                    RecipientId = recipient.Id,
                    Address = NewParcelAddress,
                    Status = NewParcelStatus,
                    CreatedAt = System.DateTime.UtcNow
                };

                db.Parcels.Add(parcel);
                db.SaveChanges();
                _ = LoadParcelsAsync();
                MessageBox.Show("Paczka została dodana.");

                // Wyczyść formularz po dodaniu
                NewParcelSender = string.Empty;
                NewParcelRecipient = string.Empty;
                NewParcelAddress = string.Empty;
                NewParcelStatus = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania paczki: {ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }

        private void EditParcel()
        {
            if (SelectedParcel == null)
                return;

            try
            {
                using var db = new AppDbContext();
                var parcel = db.Parcels.FirstOrDefault(p => p.Id == SelectedParcel.Id);
                if (parcel == null)
                {
                    MessageBox.Show("Nie znaleziono paczki.");
                    return;
                }

                parcel.Address = NewParcelAddress;
                parcel.Status = NewParcelStatus;
                db.SaveChanges();
                _ = LoadParcelsAsync();
                MessageBox.Show("Paczka została zaktualizowana.");
            }
            catch
            {
                MessageBox.Show("Błąd podczas edycji paczki.");
            }
        }

        private void FilterParcels()
        {
            _ = LoadParcelsAsync();
        }

        private void ShowHistory()
        {
            MessageBox.Show("Wyświetlanie historii paczek nie jest jeszcze zaimplementowane.");
        }
    }
}