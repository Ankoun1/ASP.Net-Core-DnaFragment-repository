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

        List<LrUsersStatisticsFormModel> UsersStatistics(byte sort);

        Task UpdateDb(string userId, string fullName, string email, string phoneNumber, string password);

        Task<User> CorrectUpdate(LrUser lrUser,string fullName,string email, string phoneNumber, string password,User user);        

        Task GeneratorPassword(string password, User user);

        void AddNewLrUserInfoDb(LrUser lrUser);

        (decimal, bool) Amount(string lrUserId);

        void UpdateUserProducts(string lrUserId,int productId,int productsCount);

        void Order(string lrUserId,string city,string address,string phoneNumber);
        
        bool IsSuperUser(string lrUserId);
        
        List<UserOrderFormModel> ShippingOrders(string lrUserId);
        
        void Received(int bagId);
        
        string GetPhoneNumber(string userId);     

    }
}
