using FoodTruckApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodTruckApp.Models;

public class NewOrder
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(100), Display(Name = "Your name/any key word")]
    public string UserName { get; set; }

    public int State { get; set; } = 1;

    public ICollection<ItemOrder> Items { get; set; }

    [MaxLength(100)]
    public string? StornoNote { get; set; }

    public int? PaymentMethod { get; set; }

    [ForeignKey("Merchant")]
    public int? MerchantId { get; set; }
}
