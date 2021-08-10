namespace DnaFragment.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LrUserStatisticsProduct
    {
        [Required]
        public int LrUserId { get; init; }
        
        public LrUser LrUser { get; set; }

        [Required]
        public int StatisticsProductId { get; init; }
        
        public StatisticsProduct StatisticsProduct { get; set; }

        public int CategoryVisitsCount { get; set; } = 0;

        public int ProductVisitsCount { get; set; } = 0;

        public int PurchasesCount { get; set; } = 0;
    }
}
