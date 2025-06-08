using System.Configuration;
using System.Data;
using System.Windows;
using WPFK.Data;

namespace WPFK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //using (var db = new AppDbContext())
            //{
            //    db.Parcels.RemoveRange(db.Parcels);
            //    db.SaveChanges();
            //}

            
        }

    }

}
