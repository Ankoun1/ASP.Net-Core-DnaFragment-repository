namespace DnaFragment.Services.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
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

        public void StartCategoryDb()
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                 new Category { Name = "ПАРФЮМИ ЗА ЖЕНИ LR", PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/guido-maria-kretscher-lr-woman.jpg" },
                 new Category { Name = "ТЕРАПИЯ ЗА ТЯЛО LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/08/thumb_body-3.jpg"},
                 new Category { Name = "ХРАНИТЕЛНИ ДОБАВКИ LR",PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/super_omega_timelr_lr.jpg" },
                 new Category { Name = "КРЕМОВЕ ЗА ЛИЦЕ LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/thumb_eye.jpg"},
                 new Category { Name = "АЛОЕ ВЕРА ГЕЛ ЗА ПИЕНЕ",PictureUrl = "https://primelr.ru/wp-content/uploads/2020/06/thumb_gel_aloe_vera_maglr-lr-immun_plus.jpg" },
                 new Category { Name = "КОЗМЕТИКА LR" ,PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/lr-colours-lipstick-care-balm-maglr.jpg"},
                 new Category { Name = "ПАРФЮМИ ЗА МЪЖЕ LR", PictureUrl = "https://primelr.ru/wp-content/uploads/2017/03/full_ocean_parfum.jpg" }
             });
            data.SaveChanges();

            StartStatisticsCategory();
        }

        private void StartStatisticsCategory()
        {
            for (int i = 1; i <= 7; i++)
            {
                data.StatisticsCategories.Add(new StatisticsCategory());
                data.SaveChanges();
            }
        }
    }
}
