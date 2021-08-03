namespace DnaFragment.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.UserConst;   
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {  
        
        [MaxLength(MaxLengthFullName)]
        public string FullName { get; set; }

        public bool IsAdministrator { get; set; }

        public string ResetPasswordId { get; set; }      
                              
        public ICollection<QuestionUser> QuestionUsers { get; init; } = new List<QuestionUser>();

        public ICollection<UserProduct> UserProducts { get; init; } = new List<UserProduct>();

        public IEnumerable<Message> Messages { get; init; } = new List<Message>();

    }
}

