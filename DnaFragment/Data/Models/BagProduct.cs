namespace DnaFragment.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BagProduct
    {
        [Required]
        public int BagId { get; init; } 

        public Bag Bag { get; init; }

        [Required]
        public int LrProductId { get; init; } 

        public LrProduct LrProduct { get; init; } 

        public int CountProducts { get; init; }
    }
}
