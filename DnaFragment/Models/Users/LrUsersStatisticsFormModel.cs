namespace DnaFragment.Models.Users
{
    using System.Collections.Generic;
    public class LrUsersStatisticsFormModel : UserListingViewModel
    {  
        public bool IsDanger { get; set; }

        public decimal? TotalSum { get; set; }

        public int? LrPoints { get; set; }

        public string CategoriesVisitsCount { get; set; }

        public string ProductsVisitsCount { get; set; }
       
    }
}
