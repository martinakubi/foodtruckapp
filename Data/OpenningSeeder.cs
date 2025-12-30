using FoodTruckApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Data;

public class OpenningSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (!await db.OpenStatus.AnyAsync())
        {
            var IsOpen = new OpenStatus
            {
                IsOpen = false,
            };

            db.OpenStatus.Add(IsOpen);
            await db.SaveChangesAsync();
        }

        if (!await db.OpenHours.AnyAsync())
        {
            var openHours = new OpenHours
            {
                Openhour = new TimeOnly(8, 0),
                Closehour = new TimeOnly(20, 0),
            };

            db.OpenHours.Add(openHours);
            await db.SaveChangesAsync();
        }
    }
}
