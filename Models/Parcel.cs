using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFK.Models
{
    public class Parcel
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ParcelStatusHistory> StatusHistories { get; set; } = new();
    }

}
