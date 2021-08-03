namespace DnaFragment.DnaFragmentControllers
{
    using System.Linq;     
    using DnaFragment.Services.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : Controller
    {
        
        private readonly CategoriesService categoriesService;

        public CategoriesController(CategoriesService categoriesService)
        {           
            
            this.categoriesService = categoriesService;
        }
       
        public IActionResult All()
        {
            var categories = categoriesService.AllCategoriesDb();
            return View(categories);
        }
      
    }
}
