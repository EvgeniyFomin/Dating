using Dating.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dating.DAL.Seed
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Dating.DAL\Seed\UserSeedData.json"));

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using FileStream openStream = File.OpenRead(path);
            var users = await JsonSerializer.DeserializeAsync<List<User>>(openStream, options);

            if (users == null) return;

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
