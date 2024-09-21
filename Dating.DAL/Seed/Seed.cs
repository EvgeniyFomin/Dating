using Dating.Core.Models;
using Dating.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Dating.DAL.Seed
{
    public static class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Dating.DAL\Seed\UserSeedData.json"));
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using FileStream openStream = File.OpenRead(path);
            var users = await JsonSerializer.DeserializeAsync<List<User>>(openStream, options);

            if (users == null) return;

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
