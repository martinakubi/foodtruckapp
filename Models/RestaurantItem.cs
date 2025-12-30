using System.ComponentModel.DataAnnotations;
using FoodTruckApp.Code;

namespace FoodTruckApp.Models;

public class RestaurantItem
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50), Display(Name = "Code to URL")]
    public string Kod { get; set; } = string.Empty;

    [Required, MaxLength(100), Display(Name = "Name of Item")]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.MultilineText), Display(Name = "Description of Item")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "~/pictures/id.png")]
    public string ImagePath { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public MealType MealType { get; set; } = MealType.Main;

    public ICollection<RestItemAlergen> Alergenes { get; set; }= new List<RestItemAlergen>();

    [Required]
    public AbilityType Ability { get; set; }
}