


namespace DnaFragment.DnaFragmentControllers.Api
{
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Models.Api.LrProducts;
    using DnaFragment.Services.LrProducts;
    using DnaFragment.Services.LrProducts.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/lrProducts")]
    public class LrProductsApiController : ControllerBase
    {
        private readonly ILrProductsService lrProducts;

        public LrProductsApiController(ILrProductsService lrProducts)
        {
            this.lrProducts = lrProducts;
        }

        [HttpGet]
        public async Task<LrProductQueryServiceModel> All([FromQuery] AllLrProductsApiRequestModel query)
        {
            return await lrProducts.All(query.Brand, query.SearchTerm, query.Sorting, query.CurrentPage, query.ProductsPerPage);        
        }
    }
}
