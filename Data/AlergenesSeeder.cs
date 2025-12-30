using FoodTruckApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Data;

public class AlergenesSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (!await db.AlergenesTypes.AnyAsync())
        {
            db.AlergenesTypes.AddRange(new List<AlergenesTypes>
        {
            new AlergenesTypes { AlergenNum = 1, AlergenDescription = "Cereals containing gluten: wheat, rye, barley, oats" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 2, AlergenDescription = "Crustaceans and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 3, AlergenDescription = "Eggs and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 4, AlergenDescription = "Fish and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 5, AlergenDescription = "Groundnuts and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 6, AlergenDescription = "Soybeans and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 7, AlergenDescription = "Milk and products thereof (containing lactose)" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 8, AlergenDescription = "Nuts: almonds, hazelnuts, walnuts, cashews, pecns, Brazil nuts, pistachios, macadamia nuts.", AlergenPicture = "a"  },
            new AlergenesTypes { AlergenNum = 9, AlergenDescription = "Celery and products thereof", AlergenPicture = "a"  },
            new AlergenesTypes { AlergenNum = 10, AlergenDescription = "Mustard and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 11, AlergenDescription = "Sesame seeds and products thereof" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 12, AlergenDescription = "Sulphur dioxide and sulphites in concentrations exceeding 10 mg/kg or 10 mg/l, expressed as total SO2" , AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 13, AlergenDescription = "Lupin and products thereof", AlergenPicture = "a" },
            new AlergenesTypes { AlergenNum = 14, AlergenDescription = "Molluscs and products thereof", AlergenPicture = "a" }
        });

            await db.SaveChangesAsync();
        }
    }
}
