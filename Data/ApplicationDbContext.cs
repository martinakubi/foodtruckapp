using Microsoft.EntityFrameworkCore;
using FoodTruckApp.Models;

namespace FoodTruckApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AdminUser> AdminUsers { get; set; } = null!;
        public DbSet<RestaurantItem> RestaurantItems { get; set; }
        public DbSet<NewOrder> NewOrders { get; set; } = null!;
        public DbSet<ItemOrder> ItemsOfOrders { get; set; } = null!;
        public DbSet<OpenStatus> OpenStatus { get; set; } = null!;
        public DbSet<OpenHours> OpenHours { get; set; } = null!;
        public DbSet<AlergenesTypes> AlergenesTypes { get; set; } = null!;
        public DbSet<RestItemAlergen> restItemAlergens { get; set; } = null!;
        public DbSet<Kennitala> Kennitalas { get; set; } = null!;
        public DbSet<Merchant> Merchant { get; set; } = null!;
    }
}