
using DnaFragment.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DnaFragment.Data
{
    public class DnaFragmentDbContext : IdentityDbContext<User>
    {
        public DnaFragmentDbContext(DbContextOptions<DnaFragmentDbContext> options)
            : base(options)
        {
        }


        public DbSet<LrUser> LrUsers { get; init; }        

        public DbSet<LrProduct> LrProducts { get; init; }

        public DbSet<Category> Categories { get; init; }

        // public DbSet<Issue> Issues { get; init; }

        public DbSet<Question> Questions { get; init; }

        public DbSet<Message> Messages { get; init; }

        public DbSet<Answer> Answers { get; init; }

        public DbSet<QuestionUser> QuestionUsers { get; init; }

        public DbSet<UserProduct> UserProducts { get; init; }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
               .Entity<LrProduct>()
               .HasOne(c => c.Category)
               .WithMany(c => c.LrProducts)
               .HasForeignKey(c => c.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserProduct>().HasKey(up => new { up.UserId, up.LrUserId, up.LrProductId });

            builder.Entity<QuestionUser>().HasKey(up => new { up.UserId, up.LrUserId, up.QuestionId });
            
            base.OnModelCreating(builder);

        }
    }
}
