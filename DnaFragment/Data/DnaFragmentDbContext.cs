

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

        public DbSet<LrProduct> LrProducts { get; init; }

        public DbSet<Category> Categories { get; init; }

        // public DbSet<Issue> Issues { get; init; }

        public DbSet<Question> Questions { get; init; }

        public DbSet<Message> Messages { get; init; }

        public DbSet<Answer> Answers { get; init; }      

        public DbSet<UserProduct> UserProducts { get; init; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
               .Entity<LrProduct>()
               .HasOne(c => c.Category)
               .WithMany(c => c.LrProducts)
               .HasForeignKey(c => c.CategoryId)
               .OnDelete(DeleteBehavior.Restrict); 

         

            builder.Entity<UserProduct>().HasKey(up => new { up.UserId, up.LrProductId });

            builder.Entity<UserProduct>()
            .HasOne<User>(tm => tm.User)
            .WithMany(tpp => tpp.UserProducts)
            .HasForeignKey(tm => tm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserProduct>()
            .HasOne<LrProduct>(tm => tm.LrProduct)
            .WithMany(tpp => tpp.UserProducts)
            .HasForeignKey(tm => tm.LrProductId)
            .OnDelete(DeleteBehavior.Cascade);
          

            builder
              .Entity<Answer>()
              .HasOne(c => c.Question)
              .WithMany(c => c.Answers)
              .HasForeignKey(c => c.QuestionId)
              .OnDelete(DeleteBehavior.Cascade); 

            builder
              .Entity<Message>()
              .HasOne(c => c.User)
              .WithMany(c => c.Messages)
              .HasForeignKey(c => c.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);

        }
    }
}
