namespace DnaFragment.DnaFragmentControllers
{   
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {      
          
        public IActionResult Index()
        {            
            /*if (this.User.IsAuthenticated)
            {
                return Redirect("/Cars/All");
            }*/       
                       
            return View();
        }
       
        public IActionResult Error() => View();
    }
}
