using Dating.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(k => k.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
                .HasOne(t => t.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(k => k.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
