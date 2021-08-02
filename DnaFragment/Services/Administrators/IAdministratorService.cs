namespace DnaFragment.Services.Administrators
{
    public interface IAdministratorService
    {
        bool UserIsRegister(string userId, bool isAdmin);

        public string AdministratorId(string userId, bool isAdmin);
    }
}
