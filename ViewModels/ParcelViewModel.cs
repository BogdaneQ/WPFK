using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFK.ViewModels
{
    internal class ParcelViewModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
