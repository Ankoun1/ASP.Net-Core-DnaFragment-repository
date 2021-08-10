using System.Collections.Generic;

namespace DnaFragment.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StatisticsCategory
    {
        public int Id { get; init; }            

        public IEnumerable<StatisticsProduct> StatisticsProducts { get; init; } = new List<StatisticsProduct>();
    }
}
