namespace DnaFragment.Services.Administrators
{
    public interface IAdministratorService
    {
        bool UserIsRegister(string userId);

        public string AdministratorId(string userId);
    }
}
