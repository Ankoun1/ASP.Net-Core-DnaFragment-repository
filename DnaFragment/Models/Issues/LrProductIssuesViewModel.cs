namespace DnaFragment.Models.Issues
{
    using System.Collections.Generic;

    public class LrProductIssuesViewModel
    {
        public string Id { get; init; }

        public string Model { get; init; }

        public int Year { get; init; }

        public bool UserIsMechanic { get; init; }

        public IEnumerable<IssueListingViewModel> Issues { get; init; }
    }
}
