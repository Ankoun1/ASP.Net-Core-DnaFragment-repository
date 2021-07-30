namespace DnaFragment.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Category
    {
        //[Key]
       // [Required]
       // [MaxLength(IdMaxLength)]
        public int Id { get; init; } //= Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public IEnumerable<LrProduct> LrProducts { get; init; } = new List<LrProduct>();
    }
}
