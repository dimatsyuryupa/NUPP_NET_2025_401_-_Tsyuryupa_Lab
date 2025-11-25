using Library.Infrastructure;
using Library.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Library.REST
{
    public static class IdentityInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Librarian", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var testUser = await userManager.FindByEmailAsync("user@test.com");
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = "user@test.com",
                    Email = "user@test.com"
                };
                await userManager.CreateAsync(testUser, "Test123!");
                await userManager.AddToRoleAsync(testUser, "User");
            }

            var adminUser = await userManager.FindByEmailAsync("admin@test.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}