namespace DnaFragment.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.LrProductConst;
    using static DataConstants.DefaultConstants;
    using System.Collections.Generic;

    public class StatisticsProduct
    {
        [Key]        
        public int Id { get; init; }

        [Required]
        [MaxLength(PlateNumberMaxLength)]
        public string PlateNumber { get; init; }       
        
        public int StatisticsCategoryId { get; init; }

        public StatisticsCategory StatisticsCategory { get; init; }

        public IEnumerable<LrUserStatisticsProduct> StatisticsCategories { get; init; } = new List<LrUserStatisticsProduct>();
    }
}
