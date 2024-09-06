using Dating.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite();

        public DbSet<User> Users { get; set; }
    }
}
