namespace DnaFragment.DnaFragmentControllers
{    
    using DnaFragment.Infrastructure;   
    using DnaFragment.Models.LrProducts;  
    using DnaFragment.Services.Administrators;   
    using System.Linq;   
    using DnaFragment.Services.LrProducts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using static WebConstants;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using DnaFragment.Services.LrProducts.Models;
 
   

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
            if (User.IsAdmin())
            {
                return Unauthorized();
            }

            string userId = User.GetId();


            //var lrProduct = lrProducts.Details(id);
          
            if (!lrProducts.ExistUserProduct(id,userId))
            {
                lrProducts.CreateUserProductFavorite(id, userId);
            }
            else
            {
                lrProducts.ProductIsFavorite(id, userId);
            }                

            return RedirectToAction(nameof(Favorits));
        }

        
        public IActionResult ForBag(int id)
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }

            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
               userId = User.GetId();
            }           

            //var lrProduct = lrProducts.Details(id);
            if(userId == null)
            {
                return Redirect(RedirectToRegister);                                        
            }
            else if (!lrProducts.ExistUserProduct(id,userId))
            {
                lrProducts.CreateUserProductBag(id, userId);
            }
            else
            {
                lrProducts.ProductIsBag(id, userId);
            }
            return RedirectToAction(nameof(Bag));
        }

        [Authorize]
        public IActionResult Favorits()
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }
           
            var products = lrProducts.Favorits(User.GetId());

            return View(products);
        } 
        
        [Authorize]
        public IActionResult Bag()
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }           
            var  products = lrProducts.LrBag(User.GetId());         

            return View(products);
        } 

        public IActionResult Details(int id)
        {
            var product = lrProducts.Details(id);

            if (!User.IsAdmin())
            {
                if (User.Identity.IsAuthenticated)
                {                   
                    lrProducts.UpdateCountVisitsProduct(User.GetName(), id);
                }
                else
                {
                    lrProducts.UpdateCountVisitsProduct("unknown@city.com", id);
                }
                
            }
            return View(product);
        }

        public IActionResult AllProductsByCategory(int categoryId)
        {
            var products = lrProducts.AllProductsByCategory(categoryId);
            if (!User.IsAdmin())
            {
                if(User.Identity.IsAuthenticated)
                {                    
                    lrProducts.UpdateCountVisitsCategory(User.GetName(), categoryId);
                }
                else
                {
                    lrProducts.UpdateCountVisitsCategory("unknown@city.com", categoryId);
                }
                
            }
            return View(products);
        }

        public async Task<IActionResult> All([FromQuery] AllProductsQueryModel query)
        {
            var queryResult = await this.lrProducts.All(query.Brand, query.SearchTerm, query.Sorting, query.CurrentPage, AllProductsQueryModel.ProductsPerPage);
            var lrBrands = this.lrProducts.AllLrBrands();

            query.TotalProducts = queryResult.TotalProducts;
            query.Brands = lrBrands;
            query.LrProducts = queryResult.LrProducts;
            query.CategoryAny = queryResult.CategoryAny;

            return View(query);
        }
               
    }
}
