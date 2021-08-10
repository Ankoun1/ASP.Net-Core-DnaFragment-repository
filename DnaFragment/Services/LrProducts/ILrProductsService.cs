
namespace DnaFragment.Services.LrProducts
{
   
    using System.Collections.Generic;
    using DnaFragment.Models;
    using DnaFragment.Services.LrProducts.Models;

    public interface ILrProductsService
    {
        public int Create(
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

        void CreateUserProduct(int productId,string userId);

        bool Update(int id, string description,decimal price, int categoryId);

        LrProductQueryServiceModel All(string brand, string searchTerm, LrProductSorting sorting, int currentPage, int productsPerPage);

        LrProductDetailsServiceModel Details(int Id);

        List<LrProductDetailsServiceModel> Favorits(string Id);

        bool ExistUserProduct(int productId, string userId);

        List<LrProductServiceModel> AllProductsByCategory(int categoryId);

        IEnumerable<string> AllLrBrands();

        IEnumerable<LrCategoryServiceModel> AllCategories();

        bool CategoryExsists(int categoryId);

        void UpdateCountVisitsCategory(string userName,int categoryId);

        void UpdateCountVisitsProduct(string userName,int id);
    }
}
