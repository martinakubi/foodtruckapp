using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace FoodTruckApp.Pages;

[AllowAnonymous]
public class AccessDeniedModel : PageModel
{ }