namespace DnaFragment.Services.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Users;

    public interface IUsersService
    {
        bool UserIsRegister(string userId);

        void SendmailForgotPassword(string email);

        bool CodCheck(int? code);

        Task ResetPasswordDb(int? code,string password);

        List<UserListingViewModel> AllUsersDb(string userId,bool isAdmin);

        void DeleteUsersDb(string userId);

        void AutomaticDeleteDb();

        User ValidEmail(string email);

    }
}
