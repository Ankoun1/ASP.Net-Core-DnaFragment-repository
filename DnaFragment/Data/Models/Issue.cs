namespace DnaFragment.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.DefaultConstants;

    public class Issue
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]       
        public string Discription { get; set; }

        public bool IsFixed { get; set; }

        [Required]       
        public string LrProductId { get; set; }

        public LrProduct LrProduct { get; set; }
    }
}

