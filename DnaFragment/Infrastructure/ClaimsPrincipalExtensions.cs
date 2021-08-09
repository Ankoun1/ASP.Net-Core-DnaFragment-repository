
namespace DnaFragment.Infrastructure
{    
    using System.Security.Claims;
    using static DnaFragment.Areas.Admin.AdminConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static string GetName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Name).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
           => user.IsInRole(AdministratorRoleName);
    }
}
