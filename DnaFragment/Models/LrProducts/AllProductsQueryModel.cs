namespace DnaFragment.Models.LrProducts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DnaFragment.Services.LrProducts;

    public class AllProductsQueryModel
    {
        public const int ProductsPerPage = 6;

        public string Brand { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public LrProductSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalProducts { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<LrProductServiceModel> LrProducts { get; set; }
    }
}
