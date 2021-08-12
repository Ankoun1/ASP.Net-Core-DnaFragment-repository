namespace DnaFragment.Infrastructure
{
    using AutoMapper;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.LrProducts;
    using DnaFragment.Models.Questions;
    using DnaFragment.Services.LrProducts.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LrProductDetailsServiceModel, AddProductUpdateFormModel>();
            //CreateMap<LrProduct, LrProductServiceModel>();
            CreateMap<LrProduct, LrProductDetailsServiceModel>();

            CreateMap<Question, QuestionListingViewModel>();
                
               
        }
    }
}
