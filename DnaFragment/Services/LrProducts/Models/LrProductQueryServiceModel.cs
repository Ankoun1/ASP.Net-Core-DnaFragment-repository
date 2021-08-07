namespace DnaFragment.Services.LrProducts.Models
{
    using System.Collections.Generic;

    public class LrProductQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int TotalProducts { get; init; }

        public int ProductsPerPage { get; init; }

        public IEnumerable<LrProductServiceModel> LrProducts { get; init; }
    }
}
