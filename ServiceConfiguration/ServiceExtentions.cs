using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FoodTruckApp.ServicesConfiguration;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.Cookie.Name = ".FoodTruckApp.AdminSession";
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", p => p.RequireRole("Admin"));
        });
    }
}
