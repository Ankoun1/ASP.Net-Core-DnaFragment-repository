namespace DnaFragment.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.LrProductConst;
    using static DataConstants.DefaultConstants;

    public class  LrProduct
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; init; }
      
        [MaxLength(3)]
        public string PackagingVolume { get; init; }

       
        public int Year { get; set; }


        public decimal Price { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(ChemicalDescriptionMaxLength)]
        public string ChemicalIngredients { get; set; }

        [Required]        
        public string PictureUrl { get; set; }

        [Required]
        [MaxLength(PlateNumberMaxLength)]
        public string PlateNumber { get; set; }

        public ICollection<UserProduct> UserProducts { get; init; } = new List<UserProduct>();

        //[Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }  

        //public IEnumerable<Issue> Issues { get; init; } = new List<Issue>();
    }
}
