namespace DnaFragment.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.LrUserConst;
    using static DataConstants.DefaultConstants;


    public class LrUser
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]      
        public string Password { get; set; }

       
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public bool IsMechanic { get; set; }
  
        public bool IsAdministrator { get; set; }

        public ICollection<QuestionUser> QuestionUsers { get; init; } = new List<QuestionUser>();

        public ICollection<UserProduct> UserProducts { get; init; } = new List<UserProduct>();

        public IEnumerable<Message> Messages { get; init; } = new List<Message>();
        
    }
}

