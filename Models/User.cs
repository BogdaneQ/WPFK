using System.ComponentModel.DataAnnotations;

namespace WPFK.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // hasła można później hashować

        public string Role {  get; set; }

        public ICollection<Parcel> Parcels { get; set; }
    }
}
