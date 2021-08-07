namespace DnaFragment.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.LrProductConst;
    using static DataConstants.DefaultConstants;

    public class StatisticsProduct
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(PlateNumberMaxLength)]
        public string PlateNumber { get; init; }

        public int? ProductVisitsCount { get; set; }

        public int? PurchasesCount { get; set; }
        
        public int StatisticsCategoryId { get; init; }

        public StatisticsCategory StatisticsCategory { get; init; }
    }
}
