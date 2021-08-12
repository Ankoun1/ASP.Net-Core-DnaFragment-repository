namespace DnaFragment.DnaFragmentControllers
{    
    using DnaFragment.Infrastructure;   
    using DnaFragment.Models.LrProducts;   
    
    using DnaFragment.Services.Administrators;
    
    using System.Linq;
    using static WebConstants;
    using DnaFragment.Services.LrProducts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    //using AutoMapper;

    public class LrProductsController : Controller
    {    
        private readonly ILrProductsService lrProducts;
        private readonly IAdministratorService administrator;
        //private readonly IMapper mapper;

        public LrProductsController(         
               ILrProductsService lrProducts, IAdministratorService administrator)
        {           
            this.lrProducts = lrProducts;
            this.administrator = administrator;
            //this.mapper = mapper;
        }

       

        [Authorize]
        public IActionResult Edit(int id)
        {
            string userId = User.GetId();
            if (administrator.UserIsRegister(userId))
            {
                return Redirect(RedirectToLogin);
            }
            //var lrProduct = lrProducts.Details(id);

            if (!lrProducts.ExistUserProduct(id,userId))
            {
                lrProducts.CreateUserProduct(id, userId);
            }         
          

            return RedirectToAction(nameof(Favorits));
        }

        [Authorize]
        public IActionResult Favorits()
        {
            if(administrator.UserIsRegister(User.GetId()))
            {
                return Unauthorized();
            }
            var products = lrProducts.Favorits(User.GetId());

            return View(products);
        } 

        public IActionResult Details(int id)
        {
            var product = lrProducts.Details(id);

            lrProducts.UpdateCountVisitsProduct(User.GetName(),id);

            return View(product);
        }

        public IActionResult AllProductsByCategory(int categoryId)
        {
            var products = lrProducts.AllProductsByCategory(categoryId);
            lrProducts.UpdateCountVisitsCategory(User.GetName(),categoryId);
            return View(products);
        }

        public IActionResult All([FromQuery] AllProductsQueryModel query)
        {
            var queryResult = this.lrProducts.All(query.Brand, query.SearchTerm, query.Sorting, query.CurrentPage, AllProductsQueryModel.ProductsPerPage);
            var lrBrands = this.lrProducts.AllLrBrands();

            query.TotalProducts = queryResult.TotalProducts;
            query.Brands = lrBrands;
            query.LrProducts = queryResult.LrProducts;

            return View(query);
        }     

    }
}
