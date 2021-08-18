namespace DnaFragment.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.LrUserConst;   
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {        
        [MaxLength(MaxLengthFullName)]
        public string FullName { get; set; }

        public bool IsAdministrator { get; set; }

        public int? ResetPasswordId { get; set; }      
                              
        public ICollection<Question> Questions { get; init; } = new List<Question>();

        public ICollection<UserProduct> UserProducts { get; init; } = new List<UserProduct>();

        public IEnumerable<Message> Messages { get; init; } = new List<Message>();

        public decimal Amount { get; set; }
        
        public decimal Count { get; set; } 
        
        public bool IsSuperUser { get; set; }     
        
    }
}

