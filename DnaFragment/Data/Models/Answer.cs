namespace DnaFragment.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.DefaultConstants;

    public class Answer
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;        

        [Required]
        [MaxLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }

        public bool StopAutomaticDelite { get; set; }

        [Required]
        public string QuestionId { get; set; }

        public Question Question { get; set; }
       
    }
}
