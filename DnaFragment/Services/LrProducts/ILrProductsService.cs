
namespace DnaFragment.Services.LrProducts
{
   
    using System.Collections.Generic;
    using DnaFragment.Models;
    using DnaFragment.Services.LrProducts.Models;

    public interface ILrProductsService
    {
        public string Create(
               string Model,
               string PackagingVolume,
               string ChemicalIngredients,
               string Description,
               decimal Price,
               int Year,
               string Image,
               string PlateNumber,
               int CategoryId, 
               string userId);

        void CreateUserProduct(string productId,string userId);

        bool Update(string id, string description,decimal price, int categoryId);

        LrProductQueryServiceModel All(string brand, string searchTerm, LrProductSorting sorting, int currentPage, int productsPerPage);

        LrProductDetailsServiceModel Details(string Id);

        List<LrProductDetailsServiceModel> Favorits(string Id);

        bool ExistUserProduct(string productId, string userId);

        List<LrProductServiceModel> AllProductsByCategory(int categoryId);

        IEnumerable<string> AllLrBrands();

        IEnumerable<LrCategoryServiceModel> AllCategories();

        bool CategoryExsists(int categoryId);
    }
}
