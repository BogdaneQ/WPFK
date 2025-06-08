using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFK.Models
{
    public class ParcelStatusHistory
    {
        public int Id { get; set; }
        public int ParcelId { get; set; }
        public string Status { get; set; }
        public DateTime ChangedAt { get; set; }

        public Parcel Parcel { get; set; }
    }

}
