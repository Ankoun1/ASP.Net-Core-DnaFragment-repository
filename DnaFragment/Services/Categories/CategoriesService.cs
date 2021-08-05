namespace DnaFragment.Services.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Models.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly DnaFragmentDbContext data;

        public CategoriesService(DnaFragmentDbContext data)
        {
            this.data = data;
        }

        public List<ListingCategoryModel> AllCategoriesDb()
        {
            var categories = data.Categories.Select(x => new ListingCategoryModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.PictureUrl
            }).ToList();

            categories.Add(new ListingCategoryModel { Name = "ВСИЧКИ ПРОДУКТИ", Image = "https://mms.businesswire.com/media/20210315005417/en/865102/5/LR_logo_invCMYK.jpg" });

            foreach (var category in categories)
            {
                if (category.Id == null)
                {
                    category.CountCategoryProducts = data.LrProducts.Count();
                }
                else
                {
                    category.CountCategoryProducts = data.LrProducts.Where(x => x.CategoryId == category.Id).Count();
                }

            }
            categories = categories.OrderBy(x => x.Id).ToList();

            return categories;
        }
    }
}
