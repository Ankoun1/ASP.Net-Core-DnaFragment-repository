namespace DnaFragment.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StatisticsLrUser
    {
        public int Id { get; init; }
        
        [EmailAddress]
        public string Email { get; set; }

        public int? CategoryVisitsCount { get; set; }

        public int? ProductVisitsCount { get; set; }

        public int? PurchasesCount { get; set; }
        
        public decimal? TotalSum { get; set; }

        public int? LrPoints { get; set; }
        
    }
}
