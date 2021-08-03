

namespace DnaFragment.Services.LrProducts
{
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models;
    using DnaFragment.Services.LrProducts;

    public class LrProductsService : ILrProductsService
    {
        private readonly DnaFragmentDbContext data;

        public LrProductsService(DnaFragmentDbContext data)
        {
            this.data = data;
        }

        public string Create(
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
                Name = model,
                PackagingVolume = packagingVolume,
                ChemicalIngredients = chemicalIngredients,
                Description = description,
                Price = price,
                Year = year,
                PictureUrl = image,
                PlateNumber = plateNumber,
                CategoryId = categoryId
            };

            
            var userProduct = new UserProduct
            {
                UserId = userId,               
                LrProductId = product.Id
            };
            product.UserProducts.Add(userProduct);

            this.data.LrProducts.Add(product);
            //this.data.UserProducts.Add(userProduct);

            this.data.SaveChanges();

            return product.Id;
        }

        public bool Update(string id, string description,decimal price,int categoryId)
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
                    (c.Name).ToLower().Contains(searchTerm.ToLower()) || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            productsQuery = sorting switch
            {
                LrProductSorting.Volume => productsQuery.OrderByDescending(c => c.PackagingVolume),
                LrProductSorting.BrandAndPrice or _ => productsQuery.OrderBy(c => c.Price).ThenBy(c => c.Name)
                //LrProductSorting.DateCreated or _ => productsQuery.OrderByDescending(c => c.Id)
            };

            var totalLrProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage)
                .Select(c => new LrProductServiceModel
                {
                    Id = c.Id,
                    Model = c.Name,
                    PackagingVolume = c.PackagingVolume,
                    Price = c.Price,
                    Image = c.PictureUrl,
                    //Category = c.Category.Name
                })
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
            var products = data.LrProducts.Where(x => x.CategoryId == categoryId).OrderBy(x => x.Price).Select(c => new LrProductServiceModel
            {
                Id = c.Id,
                Model = c.Name,
                Image = c.PictureUrl,
                PackagingVolume = c.PackagingVolume,
                Price = c.Price,
                CategoryId = c.CategoryId
            })
           .ToList();

            if (!data.LrProducts.Any(x => x.CategoryId == categoryId))
            {
                products = null;
            }

            return products;
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

        public LrProductDetailsServiceModel Details(string Id)
        => data.LrProducts.Where(x => x.Id == Id).Select(x => new LrProductDetailsServiceModel {
            Id = x.Id,
            Model = x.Name,
            PackagingVolume = x.PackagingVolume,
            Description = x.Description,
            ChemicalIngredients = x.ChemicalIngredients,
            Price = x.Price,
            Image = x.PictureUrl,
            CategoryId = x.CategoryId,
            LrUserId = Id
        }).FirstOrDefault(); 
        
        public List<LrProductDetailsServiceModel> Favorits(string id)
        =>data.LrProducts.Where(x => x.UserProducts.Where(y => y.UserId == id).Select(y => y.UserId)
            .FirstOrDefault() == id)
            .OrderBy(x => x.CategoryId)
            .ThenBy(x => x.Price)
            .Select(x => new LrProductDetailsServiceModel 
            {
              Id = x.Id,
              Model = x.Name,
              PackagingVolume = x.PackagingVolume,
              ChemicalIngredients = x.ChemicalIngredients,
              Description = x.Description,
              Image = x.PictureUrl,
              Price = x.Price
            }).ToList();

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

        public bool ExistUserProduct(string productId, string userId)
        => data.UserProducts.Any(x => x.LrProductId == productId && x.UserId == userId);

        public void CreateUserProduct(string productId, string userId)
        {
            var userProduct = new UserProduct
            {
                UserId = userId,                
                LrProductId = productId
            };

            data.UserProducts.Add(userProduct);
            data.SaveChanges();
        }
    }
}
