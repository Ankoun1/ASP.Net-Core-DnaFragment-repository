using System.ComponentModel.DataAnnotations;

namespace DnaFragment.Data.Models
{
    public class UserProduct
    {       

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        
        public int LrProductId { get; set; }

        public LrProduct LrProduct { get; set; }
    }
}
