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

            categories.Add(new ListingCategoryModel { Name = "ВСИЧКИ ПРОДУКТИ", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQIlA125LTEOuRshodgy9dsdKOHi_OaiXRQKJlZ1ytsWkpC1_cJJXnad_Y_b06osL9On8Y&usqp=CAU" });

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
