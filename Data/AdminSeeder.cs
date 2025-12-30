using FoodTruckApp.Models;
using System.Linq;

namespace FoodTruckApp.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminUser(ApplicationDbContext db, IConfiguration configuration)
        {
            var adminUsername = configuration["Login:Admin"];
            var adminPassword = configuration["Login:AdminPassword"];

            if (!db.AdminUsers.Any())
            {
                var admin = new AdminUser
                {
                    Username = adminUsername,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword)
                };

                db.AdminUsers.AddAsync(admin);
                await db.SaveChangesAsync();
            }
        }
    }
}
