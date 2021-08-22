namespace DnaFragment.Models.Users
{
    public class UserListingViewModel
    {
        public string Id { get; init; }

        public string Username { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }
        
        public bool IsAdministrator { get; init; }
        
        public bool IsSuperUser { get; init; }
        
        public int NumberMessages { get; set; }
        
        public int NumberQuestions { get; set; }
        
        public int LRPoints { get; init; }    
                  
    }
}
