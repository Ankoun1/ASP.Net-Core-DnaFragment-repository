﻿namespace DnaFragment.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LrUser
    {        
        
        public int Id { get; init; }
        
        [EmailAddress]
        public string Email { get; set; }              
        
        public decimal? TotalSum { get; set; }

        public int? LrPoints { get; set; }

        public bool IsDanger { get; set; }

        public IEnumerable<StatisticsCategory> StatisticsCategories { get; init; } = new List<StatisticsCategory>();

        public ICollection<LrUserOldEmails> OldEmails { get; set; } = new HashSet<LrUserOldEmails>();

    }
}