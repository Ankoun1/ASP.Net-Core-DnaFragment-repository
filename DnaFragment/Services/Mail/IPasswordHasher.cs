namespace DnaFragment.Services.Mail
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
