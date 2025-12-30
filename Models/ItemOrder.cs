using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodTruckApp.Data;

namespace FoodTruckApp.Models;

public class ItemOrder
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("NewOrder")]
    public int NewOrderId { get; set; }

    public NewOrder NewOrder { get; set; }

    [ForeignKey("RestaurantItem")]
    public int RestaurantItemId { get; set; }

    public RestaurantItem RestaurantItem { get; set; }

    public decimal PricePerOne { get; set; }

    [Required]
    public int NumOfDishes { get; set; }
}