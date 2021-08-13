namespace DnaFragment.DnaFragmentControllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Services.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : Controller
    {
        
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {           
            
            this.categoriesService = categoriesService;
        }
       
        public async  Task<IActionResult> All()
        {
            var categories = await categoriesService.AllCategoriesDb();
            return  View(categories);
        }
      
    }
}
