namespace FoodTruckApp.Data;

public class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, IConfiguration configuration)
    {
        await AdminSeeder.SeedAdminUser(db, configuration);
        await AlergenesSeeder.SeedAsync(db);
        await OpenningSeeder.SeedAsync(db);
    }
}
