

namespace DnaFragment.Services.LrProducts
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models;
    using DnaFragment.Services.LrProducts.Models;

    public class LrProductsService : ILrProductsService
    {
        private readonly DnaFragmentDbContext data;
        private readonly IConfigurationProvider mapper;

        public LrProductsService(DnaFragmentDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public int Create(
                string model,
                string packagingVolume,
                string chemicalIngredients,
                string description,
                decimal price,
                int year,
                string image,
                string plateNumber,
                int categoryId,
                string userId)
        {
            var product = new LrProduct
            {
                Model = model,
                PackagingVolume = packagingVolume,
                ChemicalIngredients = chemicalIngredients,
                Description = description,
                Price = price,
                Year = year,
                PictureUrl = image,
                PlateNumber = plateNumber,
                CategoryId = categoryId
            };
            
            this.data.LrProducts.Add(product);

            CreateUserProduct(product.Id, userId);           

            return product.Id;
        }

        public bool Update(int id, string description,decimal price,int categoryId)
        {
            var productData = this.data.LrProducts.Find(id);

            if (productData == null)
            {
                return false;
            }
           
            productData.Description = description;         
            productData.Price = price;         
            productData.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        }


        public LrProductQueryServiceModel All(string brand,string searchTerm, LrProductSorting sorting,int currentPage,int productsPerPage)
        {
            var productsQuery = this.data.LrProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                productsQuery = productsQuery.Where(c => c.Category.Name == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(c =>
                    (c.Model).ToLower().Contains(searchTerm.ToLower()) || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            productsQuery = sorting switch
            {
                LrProductSorting.Volume => productsQuery.OrderByDescending(c => c.PackagingVolume),
                LrProductSorting.BrandAndPrice or _ => productsQuery.OrderBy(c => c.Price).ThenBy(c => c.Model)
                //LrProductSorting.DateCreated or _ => productsQuery.OrderByDescending(c => c.Id)
            };

            var totalLrProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage).ProjectTo<LrProductServiceModel>(mapper)               
                .ToList();

            return new LrProductQueryServiceModel
            {
                TotalProducts = totalLrProducts,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                LrProducts = products
            };
        }

        public List<LrProductServiceModel> AllProductsByCategory(int categoryId)
        {
            var products = new List<LrProductServiceModel>();
            if (categoryId != 0)
            {
                products = data.LrProducts.Where(x => x.CategoryId == categoryId).OrderBy(x => x.Price).ProjectTo<LrProductServiceModel>(mapper).ToList();
                products.Insert(0, AddImageModel());
            }
            else
            {
                products.Add(AddImageModel());
            }           

            return products;
        }

        private static LrProductServiceModel AddImageModel()
        {
            return new LrProductServiceModel
            {
                Photos = new List<string> { "https://c4.wallpaperflare.com/wallpaper/74/530/410/sweet-girl-pic-3840x2160-wallpaper-preview.jpg",
                "https://media.gettyimages.com/photos/bikini-woman-napping-in-a-hammock-at-the-caribbean-beach-picture-id125145258?k=6&m=125145258&s=612x612&w=0&h=kyver0PULttVYsRqiFCCwQ66DySg3SJj7bodsSMG83A=",
                "https://c4.wallpaperflare.com/wallpaper/577/412/12/hot-girl-pic-1920x1200-wallpaper-preview.jpg",
                "https://c4.wallpaperflare.com/wallpaper/869/515/658/pic-girl-2560x1600-wallpaper-preview.jpg",
                "https://wallup.net/wp-content/uploads/2019/09/473320-landscape-view-height-city-dal-beauty-wind-girl-sunset.jpg",
                "https://c4.wallpaperflare.com/wallpaper/898/902/381/pic-girl-2560x1600-wallpaper-thumb.jpg",
                "https://c4.wallpaperflare.com/wallpaper/898/902/381/pic-girl-2560x1600-wallpaper-thumb.jpg" }
            };
        }

        public IEnumerable<string> AllLrBrands()
        {
            return data
                .LrProducts
                .Select(c => c.Category.Name)
                .Distinct()
                .OrderBy(br => br)
                .ToList();
        }

        public LrProductDetailsServiceModel Details(int Id)
        => data.LrProducts.Where(x => x.Id == Id).ProjectTo<LrProductDetailsServiceModel>(mapper)
           .FirstOrDefault(); 
        
        public List<LrProductDetailsServiceModel> Favorits(string id)
        =>data.LrProducts.Where(x => x.UserProducts.Where(y => y.UserId == id).Select(y => y.UserId)
            .FirstOrDefault() == id)
            .OrderBy(x => x.CategoryId)
            .ThenBy(x => x.Price)
            .ProjectTo<LrProductDetailsServiceModel>(mapper)
            .ToList();

        public IEnumerable<LrCategoryServiceModel> AllCategories()        
           => this.data
               .Categories
               .Select(c => new LrCategoryServiceModel
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToList();

        public bool CategoryExsists(int categoryId)        
        =>  this.data.Categories.Any(c => c.Id == categoryId);

        public bool ExistUserProduct(int productId, string userId)
        => data.UserProducts.Any(x => x.LrProductId == productId && x.UserId == userId);

        public void CreateUserProduct(int productId, string userId)
        {
            var userProduct = new UserProduct
            {
                UserId = userId,                
                LrProductId = productId
            };

            data.UserProducts.Add(userProduct);
            data.SaveChanges();
        }

        public void UpdateCountVisitsCategory(string userName,int categoryId)
        {
            var userId = data.LrUsers.Where(x => x.Email == userName).Select(x => x.Id).FirstOrDefault();
            var statisticsCategory = data.StatisticsCategories.Where(x => x.Id == categoryId).FirstOrDefault();
            var statisticsProducts = data.LrUserStatisticsProducts.Where(x => x.StatisticsProduct.StatisticsCategoryId == statisticsCategory.Id).ToList();

            foreach (var statisticsProduct in statisticsProducts)
            {
                statisticsProduct.CategoryVisitsCount++;
                data.SaveChanges();
            }           
            
        }

        public void UpdateCountVisitsProduct(string userName,int id)
        {
            var userId = data.LrUsers.Where(x => x.Email == userName).Select(x => x.Id).FirstOrDefault();
            var statisticsProduct = data.LrUserStatisticsProducts.Where(x => x.LrUserId == userId && x.StatisticsProductId == id).FirstOrDefault();
            
            statisticsProduct.ProductVisitsCount++;
            data.SaveChanges();
        }
    }
}
