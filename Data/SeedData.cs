using AracKiralamaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AracKiralamaAPI.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(
            UserManager<AppUser>    userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Rolleri oluştur
            foreach (var role in new[] { "Admin", "User" })
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // Admin kullanıcı oluştur
            const string adminEmail = "admin@carrental.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new AppUser
                {
                    FirstName      = "Admin",
                    LastName       = "User",
                    Email          = adminEmail,
                    UserName       = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
