namespace DnaFragment.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Bag
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string UserId { get; init; }

        [Required]
        public string ShippingAddress { get; init; } 

        [Required]
        public string City { get; init; }

        [Required]
        public string PhoneNumber { get; init; }

        public decimal Total { get; init; }

        public ICollection<BagProduct> BagProducts { get; init; } = new List<BagProduct>();
    }
}
