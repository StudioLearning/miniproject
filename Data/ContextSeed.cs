using Microsoft.AspNetCore.Identity;
using miniproject.Models;

public static class ContextSeed
{
    public static async Task SeedRolesAsync(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager
    ){
        //Seed Roles
        if(!await roleManager.RoleExistsAsync(Roles.Admin.ToString())) {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        }
        if(!await roleManager.RoleExistsAsync(Roles.User.ToString())) {
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }
    }

    public static async Task SeedSuperAdminAsync(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager
    ){
        //Seed Default User
        string mail = "admin@studiolearning.online";
        var defaultUser = new ApplicationUser 
        { 
            UserName = mail, 
            Email = mail,
            // FirstName = "code",
            // LastName = "tom",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if(user == null)
            {
                await userManager.CreateAsync(defaultUser, "Qwerty@9");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }   
        }
    }
}