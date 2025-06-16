using System.Collections.ObjectModel;
using System.Linq;
using WPFK.Data;
using WPFK.Models;

namespace WPFK.ViewModels
{
    public class StatusHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<ParcelStatusHistory> History { get; }

        public StatusHistoryViewModel(int parcelId)
        {
            using var db = new AppDbContext();
            var history = db.ParcelStatusHistories
                .Where(h => h.ParcelId == parcelId)
                .OrderByDescending(h => h.ChangedAt)
                .ToList();

            History = new ObservableCollection<ParcelStatusHistory>(history);
        }
    }
}