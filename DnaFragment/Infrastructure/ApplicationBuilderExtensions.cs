namespace DnaFragment.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using static DnaFragment.Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
       
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedAdministrator(services);                   

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<DnaFragmentDbContext>();

            data.Database.Migrate();
        }        
      

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string admin1Email = "ankonikolchevpl@gmail.com";
                    const string admin1Password = "admin123";

                    var user1 = new User
                    {
                        Email = admin1Email,
                        UserName = admin1Email,
                        FullName = "Admin1",
                        IsAdministrator = true
                    }; 
                    const string admin2Email = "niki@gmail.com";
                    const string admin2Password = "admin1234";

                    var user2 = new User
                    {
                        Email = admin2Email,
                        UserName = admin2Email,
                        FullName = "Admin2",
                        IsAdministrator = true
                    };

                    await userManager.CreateAsync(user1, admin1Password);
                    await userManager.AddToRoleAsync(user1, role.Name);

                    await userManager.CreateAsync(user2, admin2Password);
                    await userManager.AddToRoleAsync(user2, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}


