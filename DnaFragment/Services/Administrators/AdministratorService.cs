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

        public string AdministratorId(string userId,bool isAdmin)
       => data
              .Users
              .Where(x => x.Id == userId && isAdmin)
              .Select(x => x.Id)
              .FirstOrDefault();

        public bool UserIsRegister(string userId, bool isAdmin)
       => this.data
                .Users
                .Any(d => d.Id == userId && isAdmin);

    }
}
