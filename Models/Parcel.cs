using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFK.Models
{
    public class Parcel
    {
        public int Id { get; set; }

        // Foreign key for Sender
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        // Foreign key for Recipient
        public int RecipientId { get; set; }
        [ForeignKey(nameof(RecipientId))]
        public User Recipient { get; set; }

        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ParcelStatusHistory> StatusHistories { get; set; } = new();
    }
}