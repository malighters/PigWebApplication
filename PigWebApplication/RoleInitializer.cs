using Microsoft.AspNetCore.Identity;
using PigWebApplication.Models;

namespace PigWebApplication
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "ttp@email.com";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("vet") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("vet"));
            }
            if (await roleManager.FindByNameAsync("worker") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("worker"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
