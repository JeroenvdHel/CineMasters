using CineMasters.Areas.Accounts.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Config
{
    public static class IdentitySeedData
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            RoleManager<IdentityRole> roleManager =
                app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<AppUser> userManager =
                app.ApplicationServices.GetRequiredService<UserManager<AppUser>>();

            IdentityRole role = await roleManager.FindByNameAsync("Administrator");
            if (role == null)
            {
                role = new IdentityRole("Administrator");
                await roleManager.CreateAsync(role);
            }

            role = await roleManager.FindByNameAsync("Analyst");
            if (role == null)
            {
                role = new IdentityRole("Analyst");
                await roleManager.CreateAsync(role);
            }

            AppUser user = await userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                user = new AppUser { UserName = "Admin"};
                user.Email = "admin@cinemaster.nl";
                await userManager.CreateAsync(user, "Testing123!");
                await userManager.AddToRoleAsync(user, "Administrator");
            }

            user = await userManager.FindByNameAsync("Backoffice1");
            if (user == null)
            {
                user = new AppUser {UserName = "Backoffice1"};
                user.Email = "backoffice1@cinemaster.nl";
                await userManager.CreateAsync(user, "Testing123!");
                await userManager.AddToRoleAsync(user, "Back-office mederwerker");
            }

            user = await userManager.FindByNameAsync("Kassa1");
            if (user == null)
            {
                user = new AppUser { UserName = "Kassa1" };
                user.Email = "kassa1@cinemaster.nl";
                await userManager.CreateAsync(user, "Testing123!");
                await userManager.AddToRoleAsync(user, "Kassamederwerker");
            }
        }
    }
}
