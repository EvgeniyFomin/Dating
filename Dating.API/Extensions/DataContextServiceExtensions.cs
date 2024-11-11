using Dating.Core.Models;
using Dating.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Role = Dating.Core.Models.Identity.Role;

namespace Dating.API.Extensions
{
    public static class DataContextServiceExtensions
    {
        public static IServiceCollection AddDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(
                    configuration.GetConnectionString("SQLite"),
                    options => options.MigrationsAssembly("Dating.DAL"));
            });

            return services;
        }
    }
}
