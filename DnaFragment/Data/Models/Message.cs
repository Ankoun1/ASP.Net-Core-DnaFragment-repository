namespace DnaFragment.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.DefaultConstants;
    public class Message
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTime CreatedOn { get; init; } = DateTime.Now;

        public bool StopAutomaticDelete { get; set; }

        [Required]
        [MaxLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string LrUserId { get; init; }

        public LrUser LrUser { get; init; }

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }
    }
}
