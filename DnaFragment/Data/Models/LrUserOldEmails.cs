

namespace DnaFragment.Data.Models
{
    public class LrUserOldEmails
    {
        public int Id { get; init; }
        
        public string Email { get; init; }

        public int LrUserId { get; init; }

        public LrUser LrUser { get; init; }
    }
}
