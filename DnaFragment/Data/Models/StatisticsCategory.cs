using System.Collections.Generic;

namespace DnaFragment.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StatisticsCategory
    {
        public int Id { get; init; }

        public int? CategoryVisitsCount { get; set; }

       
        public int LrUserId { get; init; }

        public LrUser LrUser { get; init; }

        public IEnumerable<StatisticsProduct> StatisticsProducts { get; init; } = new List<StatisticsProduct>();
    }
}
