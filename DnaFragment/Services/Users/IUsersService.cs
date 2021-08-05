namespace DnaFragment.Services.Users
{
    using System.Collections.Generic;
    using DnaFragment.Models.Users;

    public interface IUsersService
    {
        void SendmailForgotPassword(string email);

        bool CodCheck(int? code);

        void ResetPasswordDb(int? code,string password);

        List<UserListingViewModel> AllUsersDb(string userId,bool isAdmin);

        void DeleteUsersDb(string userId);
    }
}
