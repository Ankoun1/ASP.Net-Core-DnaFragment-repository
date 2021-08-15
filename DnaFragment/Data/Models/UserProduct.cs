using System.ComponentModel.DataAnnotations;

namespace DnaFragment.Data.Models
{
    public class UserProduct
    {       

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }

        
        public int LrProductId { get; init; }

        public LrProduct LrProduct { get; init; }

        public bool InTheBag { get; set; }

        public bool InFavorits { get; set; }

        public int LrProductsCount { get; set; }

    }
}
