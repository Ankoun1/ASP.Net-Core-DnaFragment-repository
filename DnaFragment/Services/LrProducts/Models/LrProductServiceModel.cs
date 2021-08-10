

namespace DnaFragment.Services.LrProducts.Models
{
    using System.Collections.Generic;

    public class LrProductServiceModel
    {
        public int Id { get; init; }

        public string Model { get; init; }

        public string PictureUrl { get; init; }

        public string PackagingVolume { get; init; }

        public decimal Price { get; init; }

        public int CategoryId { get; init; }

        public ICollection<string> Photos { get; set; } = new List<string>();
    }
}
