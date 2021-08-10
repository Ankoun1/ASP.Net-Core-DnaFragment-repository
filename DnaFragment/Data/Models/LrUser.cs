namespace DnaFragment.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LrUser
    {        
        
        public int Id { get; init; }
        
        [EmailAddress]
        public string Email { get; set; }

        public decimal TotalSum { get; set; } = 0;

        public int LrPoints { get; set; } = 0;

        public bool IsDanger { get; set; }

        public IEnumerable<LrUserStatisticsProduct> StatisticsCategories { get; init; } = new List<LrUserStatisticsProduct>();

        public ICollection<LrUserOldEmails> OldEmails { get; set; } = new HashSet<LrUserOldEmails>();

    }
}
