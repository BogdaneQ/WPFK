using System.Collections.ObjectModel;
using System.Linq;
using WPFK.Models;
using WPFK.Data;

namespace WPFK.ViewModels
{
    public class DeliveredParcelViewModel
    {
        public string SenderName { get; set; }
        public string Address { get; set; }
        public string DeliveredAt { get; set; }
    }

    public class UserDeliveredHistoryViewModel
    {
        public ObservableCollection<DeliveredParcelViewModel> DeliveredParcels { get; } = new();

        public UserDeliveredHistoryViewModel(int userId)
        {
            using var db = new AppDbContext();
            var delivered = db.Parcels
                .Where(p => p.RecipientId == userId && p.Status == "Dostarczona")
                .Select(p => new DeliveredParcelViewModel
                {
                    SenderName = p.Sender.Username,
                    Address = p.Address,
                    DeliveredAt = p.CreatedAt.ToString("g")
                })
                .ToList();

            foreach (var parcel in delivered)
                DeliveredParcels.Add(parcel);
        }
    }
}