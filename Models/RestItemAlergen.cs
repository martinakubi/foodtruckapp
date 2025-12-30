using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodTruckApp.Data;

namespace FoodTruckApp.Models;

public class RestItemAlergen
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("RestaurantItem")]
    public int RestaurantItemId { get; set; }
    public RestaurantItem RestaurantItem { get; set; }

    [ForeignKey("AlergenesTypes")]
    public int AlergenesTypesId { get; set; }
    public AlergenesTypes AlergenesTypes { get; set; }
}
