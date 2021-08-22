namespace DnaFragment.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserProduct
    {      
        [Required]
        public string UserId { get; init; }

        public User User { get; init; }
        
        public int LrProductId { get; init; }

        public LrProduct LrProduct { get; init; }

        public bool InTheBag { get; set; }

        public bool InFavorits { get; set; }

        public bool Bought { get; set; }

        public int LrProductsCount { get; set; }

        public decimal Amount { get; set; }

        public decimal PercentageDiscount { get; set; }
    }
}
