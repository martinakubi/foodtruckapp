using System.ComponentModel.DataAnnotations;

namespace FoodTruckApp.Models;

public class Merchant
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required."), MaxLength(50), Display(Name = "Name of the company.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required."), MaxLength(150), Display(Name = "Adress of the company.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required."), MaxLength(15), Display(Name = "Number with an area code")]
    [RegularExpression(@"^\+?[0-9 ]{7,15}$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required."), MaxLength(50), Display(Name = "Email adress connected to company.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kennitala is required."), MaxLength(20), Display(Name = "Kennitala number of the company.")]
    public string Kennitala { get; set; } = string.Empty;

    [Required(ErrorMessage = "VSK number is required."), MaxLength(10), Display(Name = "VSK number of the company.")]
    public string VSKNumber { get; set; } = string.Empty;

    [Required, Display(Name = "https://www.google.com/maps/embed?...")]
    public string MapEmbed { get; set; } = string.Empty ;

    [Required, Display(Name = "Trip Advisor")]
    public string TripAdviEmbed { get; set; } = string.Empty;

    [Required, Display(Name = "Google")]
    public string GoogleEmbed { get; set; } = string.Empty;
}
