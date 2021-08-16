namespace DnaFragment.Models.LrProducts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DnaFragment.Services.LrProducts.Models;

    public class AddNewInfoProductFormModel
    {      
        public List<LrProductDetailsServiceModel> Products { get; init; }

        [Range(1, 100)]
        public int ProductsCount { get; init; } = 1;

        public bool  ProductsCountIsNotEmpty { get; init; }

        public decimal? Amount { get; init; }
    }
}
