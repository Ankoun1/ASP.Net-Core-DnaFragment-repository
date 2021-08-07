namespace DnaFragment.Models.LrProducts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;  
    using static Data.DataConstants.LrProductConst;
    using static Data.DataConstants.DefaultConstants;
    using DnaFragment.Services.LrProducts.Models;

    public class AddProductFormModel
    {
        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
        public string Model { get; init; }

        [Required]
        [StringLength(MaxVolume, MinimumLength =MinVolume)]
        public string PackagingVolume { get; init; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(
        DescriptionMaxLength,
        MinimumLength = DescriptionMinLength,
        ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; set; }


        [Required]
        [StringLength(
        ChemicalDescriptionMaxLength,
        MinimumLength = DescriptionMinLength,
        ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string ChemicalIngredients { get; set; }

        public int Year { get; init; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string Image { get; init; }

        public string PlateNumber { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<LrCategoryServiceModel> Categories { get; set; }
    }
}


