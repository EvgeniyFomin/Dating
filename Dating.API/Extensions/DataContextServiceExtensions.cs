using Dating.Core.Models;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Dating.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Role = Dating.Core.Models.Identity.Role;

namespace Dating.API.Extensions
{
    public static class DataContextServiceExtensions
    {
        public static IServiceCollection AddDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IPhotosRepository, PhotosRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(
                    configuration.GetConnectionString("SqlConnection"),
                    options => options.MigrationsAssembly("Dating.DAL"));
            });

            return services;
        }
    }
}
