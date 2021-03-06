
namespace DnaFragment
{
   
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.DnaFragmentControllers;
    using DnaFragment.Infrastructure;
    using DnaFragment.Services.Administrators;
    using DnaFragment.Services.Answers;
    using DnaFragment.Services.Categories;
    using DnaFragment.Services.LrProducts;
    using DnaFragment.Services.Mail;
    using DnaFragment.Services.Messages;
    using DnaFragment.Services.Questions;
    using DnaFragment.Services.Users;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)        
          =>  Configuration = configuration;
       

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<DnaFragmentDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options =>
               {
                   options.Password.RequireDigit = false;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = false;
               })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DnaFragmentDbContext>();

            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();

            services.AddControllersWithViews(options => {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IUsersService,UsersService>();
            services.AddTransient<ILrProductsService,LrProductsService>();
            services.AddTransient<IQuestionsService,QuestionsService>();
            services.AddTransient<IAnswersService,AnswersService>();
            services.AddTransient<IAdministratorService, AdministratorService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
           
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");               
                app.UseHsts();
            }
            app.UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapControllerRoute(name: "default", pattern: "{controller=Messages}/{action=SendMail}/{id?}"); 
                endpoints.MapRazorPages();
            });
        }
    }
}
