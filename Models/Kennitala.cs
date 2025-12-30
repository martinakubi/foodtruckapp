using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FoodTruckApp.Data;

namespace FoodTruckApp.Models;

public class Kennitala
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int NewOrderId { get; set; }

    [ForeignKey(nameof(NewOrderId))]
    public NewOrder NewOrder { get; set; }

    [Required, MaxLength(20), Display(Name = "Kennitala Number")]
    public string? KennitalaNumber { get; set; }
    
    [MaxLength(40), Display(Name = "Name of company")]
    public string? KennitalaName { get; set; }
}
