namespace DnaFragment.Services.Categories
{
    using System.Collections.Generic;
    using DnaFragment.Models.Categories;

    public interface ICategoriesService
    {
        List<ListingCategoryModel> AllCategoriesDb();
    }
}
