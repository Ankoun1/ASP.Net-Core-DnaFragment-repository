namespace DnaFragment.Areas.Admin.Controllers
{
    using DnaFragment.Models.LrProducts;
    using DnaFragment.Services.LrProducts;
    using Microsoft.AspNetCore.Mvc;

    public class LrProductsController : AdminController
    {
        
        private readonly ILrProductsService lrProducts;
        public LrProductsController(ILrProductsService lrProducts)
        {
            this.lrProducts = lrProducts;
            
        }

      
        public IActionResult Start()
        {
            lrProducts.StartLrProduktDb();

            return Redirect("~/LrProducts/All");
        }
        
        public IActionResult Add(int id)
        {         
            if (id != 0)
            {
                var product = lrProducts.Details(id);

                return View(new AddProductFormModel
                {
                    Model = product.Model,
                    Description = product.Description,
                    ChemicalIngredients = product.ChemicalIngredients,
                    Price = product.Price,
                    PlateNumber = product.PlateNumber,
                    Categories = lrProducts.AllCategories()
                });
            }
            else
            {
                return View(new AddProductFormModel
                {
                    Categories = lrProducts.AllCategories()
                });
            }
        }

        [HttpPost]      
        public IActionResult Add(int id, AddProductFormModel lrProduct)
        {           
            if (!lrProducts.CategoryExsists(lrProduct.CategoryId))
            {
                this.ModelState.AddModelError(nameof(lrProduct.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                lrProduct.Categories = this.lrProducts.AllCategories();
                return View(lrProduct);
            }

            if (id != 0)
            {
                lrProducts.Update(
                lrProduct.Description,
                lrProduct.Price,
                lrProduct.Image,
                lrProduct.PlateNumber,
                id,
                lrProduct.CategoryId);
            }
            else
            {
                lrProducts.Create(lrProduct.Model,
                lrProduct.PackagingVolume,
                lrProduct.ChemicalIngredients,
                lrProduct.Description,
                lrProduct.Price,
                lrProduct.Year,
                lrProduct.Image,
                lrProduct.PlateNumber,
                lrProduct.CategoryId);
            }

            return Redirect("~/LrProducts/All");
        }

    }
}
