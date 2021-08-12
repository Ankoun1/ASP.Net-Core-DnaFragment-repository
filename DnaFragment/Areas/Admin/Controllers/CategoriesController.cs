
using DnaFragment.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace DnaFragment.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

       
        public IActionResult Start()
        {
            categoriesService.StartCategoryDb();
            return Redirect("~/LrProducts/All");
        }
    }
}
