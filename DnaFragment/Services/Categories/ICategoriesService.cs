namespace DnaFragment.Services.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DnaFragment.Models.Categories;

    public interface ICategoriesService
    {
        Task<List<ListingCategoryModel>> AllCategoriesDb();

        Task StartCategoryDb();
    }
}
