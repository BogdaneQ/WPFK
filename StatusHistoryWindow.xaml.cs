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

namespace WPFK
{
    /// <summary>
    /// Logika interakcji dla klasy StatusHistoryWindow.xaml
    /// </summary>
    public partial class StatusHistoryWindow : Window
    {
        public StatusHistoryWindow(int parcelId)
        {
            InitializeComponent();

            using var db = new AppDbContext();
            var history = db.ParcelStatusHistories
                            .Where(h => h.ParcelId == parcelId)
                            .OrderByDescending(h => h.ChangedAt)
                            .ToList();

            StatusHistoryListView.ItemsSource = history;
        }
    }

}
