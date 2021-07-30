namespace DnaFragment.Services.Administrators
{
    using System.Linq;
    using DnaFragment.Data;

    public class AdministratorService : IAdministratorService
    {
        private readonly DnaFragmentDbContext data;

        public AdministratorService(DnaFragmentDbContext data)
        {
            this.data = data;
        }

        public string AdministratorId(string userId)
       => data
              .LrUsers
              .Where(x => x.Id == userId && x.IsAdministrator)
              .Select(x => x.Id)
              .FirstOrDefault();

        public bool UserIsRegister(string userId)
       => this.data
                .LrUsers
                .Any(d => d.Id == userId && d.IsAdministrator);

    }
}
