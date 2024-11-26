using Dating.Core.Models;
using Dating.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dating.DAL.Seed
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Dating.DAL\Seed\UserSeedData.json"));

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using FileStream openStream = File.OpenRead(path);
            var users = await JsonSerializer.DeserializeAsync<List<User>>(openStream, options);

            if (users == null) return;

            var roles = new List<Role>
            {
                new(){Name = "Member"},
                new(){Name = "Admin"},
                new(){Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.Photos.First(x => x.IsMain).IsApproved = true;
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new User
            {
                UserName = "admin",
                KnownAs = "Admin",
                City = "",
                Country = ""
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
        }
    }
}
