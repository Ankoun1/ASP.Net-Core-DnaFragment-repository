namespace DnaFragment.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class QuestionUser
    {
        [Required]
        public string QuestionId { get; set; }

        public Question Question { get; set; }
       

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

    }
}
