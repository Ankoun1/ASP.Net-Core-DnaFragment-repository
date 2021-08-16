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
    using DnaFragment.Services.Users;



    //using AutoMapper;

    public class LrProductsController : Controller
    {    
        private readonly ILrProductsService lrProducts;
        private readonly IAdministratorService administrator;
        private readonly IUsersService userService;
       
        //private readonly IMapper mapper;
        
        public LrProductsController(         
               ILrProductsService lrProducts, IAdministratorService administrator, IUsersService userService)
        {           
            this.lrProducts = lrProducts;
            this.administrator = administrator;
            this.userService = userService;
            
           
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

            return View(new AddNewInfoProductFormModel {Amount = userService.Amount(User.GetId()).Item1,ProductsCountIsNotEmpty = userService.Amount(User.GetId()).Item2, Products = products });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Bag(int id, AddNewInfoProductFormModel product)
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }
            if(product.ProductsCount <= 0)
            {
                return Redirect("/LrProducts/Bag");
            }
            if (!ModelState.IsValid)
            {
             
                return View(product);
            }
            userService.UpdateUserProducts(User.GetId(), id, product.ProductsCount);

            return Redirect("/LrProducts/Bag");
        }
               [Authorize]
        public IActionResult BagsInformation()
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }               

            return View();
        }
        
      
         [HttpGet]
         [Authorize]
        public IActionResult BagsInformation(BagsInformationsModel userBag)
        {
            if (User.IsAdmin())
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {

                return View(userBag);
            }
            userService.Order(User.GetId(), userBag.City, userBag.Address, userBag.PhoneNumber);
            return Redirect("/LrProducts/Bag");
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

        [Authorize]
        public IActionResult DeleteFromTheBag(int id)
        {
            var product = lrProducts.Details(id);

            if (!User.IsAdmin())
            {               
               lrProducts.BagDelete(id,User.GetId());

            }
            else
            {
                return Unauthorized();
            }
            return Redirect("/LrProducts/Bag");
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
