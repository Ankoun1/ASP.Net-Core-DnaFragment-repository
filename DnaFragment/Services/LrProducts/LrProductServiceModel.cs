namespace DnaFragment.Services.LrProducts
{
    public class LrProductServiceModel
    {
        public string Id { get; init; }

        public string Model { get; init; }

        public string Image { get; init; }

        public string PackagingVolume { get; init; }

        public decimal Price { get; init; }

        public int CategoryId { get; init; }
    }
}
