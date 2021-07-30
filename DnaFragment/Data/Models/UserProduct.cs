using System.ComponentModel.DataAnnotations;

namespace DnaFragment.Data.Models
{
    public class UserProduct
    {
        [Required]
        public string LrUserId { get; set; }

        public LrUser LrUser { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string LrProductId { get; set; }

        public LrProduct LrProduct { get; set; }
    }
}
