

namespace DnaFragment.Models.Api.LrProducts
{
    public class AllLrProductsApiRequestModel
    {
        public string Brand { get; init; }

        public int ProductsPerPage { get; init; } = 6;

        public string SearchTerm { get; init; }

        public LrProductSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalProducts { get; init; }
    }
}
