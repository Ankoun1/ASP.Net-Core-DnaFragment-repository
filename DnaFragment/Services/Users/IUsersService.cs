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

        UserListingViewModel UsersDb(string userId);

        void DeleteUsersDb(string userId,bool isAdmin);

        void AutomaticDeleteDb();

        User ValidEmail(string email);

        List<LrUsersStatisticsFormModel> UsersStatistics();

        Task UpdateDb(string userId, string fullName, string email, string phoneNumber, string password);

        Task<User> CorrectUpdate(LrUser lrUser,string fullName,string email, string phoneNumber, string password,User user);        

        Task GeneratorPassword(string password, User user);

        void AddNewLrUserInfoDb(LrUser lrUser);

       

    }
}
