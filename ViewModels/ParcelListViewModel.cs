using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFK.Data;
using WPFK.Helpers;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using Microsoft.Win32;
using PdfSharp.Drawing;


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
            set
            {
                if (_filterStatus != value)
                {
                    _filterStatus = value;
                    OnPropertyChanged();
                    _ = LoadParcelsAsync(); 
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddParcelCommand { get; }
        public ICommand EditParcelCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ShowHistoryCommand { get; }
        public ICommand DeleteParcelCommand { get; }

        public ICommand ShowDeliveredHistoryCommand { get; }
        public ICommand GeneratePdfCommand { get; }


        public ParcelListViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await LoadParcelsAsync());
            AddParcelCommand = new RelayCommand(AddParcel);
            EditParcelCommand = new RelayCommand(EditParcel, () => SelectedParcel != null);
            FilterCommand = new RelayCommand(FilterParcels);
            ShowHistoryCommand = new RelayCommand(ShowHistory);
            DeleteParcelCommand = new RelayCommand(DeleteParcel, () => SelectedParcel != null);
            ShowDeliveredHistoryCommand = new RelayCommand(ShowDeliveredHistory);
            _ = LoadParcelsAsync();
            GeneratePdfCommand = new RelayCommand(GeneratePdf);

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
            if (SelectedParcel == null)
            {
                MessageBox.Show("Wybierz paczkę, aby zobaczyć historię.");
                return;
            }

            MessageBox.Show($"Status: {SelectedParcel.Status}\nData utworzenia: {SelectedParcel.CreatedAt:g}", "Historia paczki");
        }


        private void GeneratePdf()
        {
            if (Parcels.Count == 0)
            {
                MessageBox.Show("Brak paczek do wyeksportowania.");
                return;
            }

            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = "ListaPaczek.pdf"
                };

                if (dlg.ShowDialog() != true)
                    return;

                using var document = new PdfDocument();
                document.Info.Title = "Lista przesyłek";

                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);
                var fontBold = new XFont("Arial", 18);



                double yPoint = 40;

                gfx.DrawString("Lista przesyłek", fontBold, XBrushes.Black,
                    new XRect(0, yPoint, page.Width, page.Height),
                    XStringFormats.TopCenter);
                yPoint += 40;

                foreach (var parcel in Parcels)
                {
                    string line = $"Nadawca: {parcel.SenderName} | Odbiorca: {parcel.RecipientName} | " +
                                  $"Adres: {parcel.Address} | Status: {parcel.Status} | Data: {parcel.CreatedAt:g}";

                    gfx.DrawString(line, font, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, page.Height), XStringFormats.TopLeft);
                    yPoint += 25;

                    if (yPoint > page.Height - 40)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPoint = 40;
                    }
                }

                document.Save(dlg.FileName);

                MessageBox.Show("PDF został wygenerowany.");
                Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas generowania PDF: {ex.Message}");
            }
        }




        private void ShowDeliveredHistory()
        {
            var userId = Session.CurrentUser?.Id ?? 0;
            var historyView = new WPFK.Views.Users.UserDeliveredHistory(userId);

            var window = new Window
            {
                Title = "Historia dostarczonych przesyłek",
                Content = historyView,
                Width = 600,
                Height = 400,
                DataContext = new WPFK.ViewModels.UserDeliveredHistoryViewModel(userId)
            };
            window.ShowDialog();
        }
        private void DeleteParcel()
        {
            if (SelectedParcel == null)
                return;

            var result = MessageBox.Show("Czy na pewno chcesz usunąć tę paczkę?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
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

                db.Parcels.Remove(parcel);
                db.SaveChanges();
                _ = LoadParcelsAsync();
                MessageBox.Show("Paczka została usunięta.");
            }
            catch
            {
                MessageBox.Show("Błąd podczas usuwania paczki.");
            }
        }

    }
}