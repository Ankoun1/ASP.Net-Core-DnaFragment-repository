
namespace DnaFragment.Models.LrProducts
{    
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.LrProductConst;
    using static Data.DataConstants.DefaultConstants;
    using System.Collections.Generic;
    using DnaFragment.Services.LrProducts;

    public class AddProductUpdateFormModel
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(
        DescriptionMaxLength,
        MinimumLength = DescriptionMinLength,
        ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<LrCategoryServiceModel> Categories { get; set; }
    }
}
