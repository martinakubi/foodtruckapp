using System.ComponentModel.DataAnnotations;

namespace FoodTruckApp.Models;

public class AlergenesTypes
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AlergenNum { get; set; }
    [Required]
    public string AlergenDescription { get; set; }
    public string AlergenPicture { get; set; }

}