namespace DnaFragment.Models.Users
{
    using System.Collections.Generic;
    public class LrUsersStatisticsFormModel : UserListingViewModel
    {  
        public bool IsDanger { get; init; }      

        public string LrPoints { get; init; }

        public string CategoriesVisitsCount { get; set; }

        public string ProductsVisitsCount { get; set; }
       
    }
}
