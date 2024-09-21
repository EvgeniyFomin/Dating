using Dating.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
