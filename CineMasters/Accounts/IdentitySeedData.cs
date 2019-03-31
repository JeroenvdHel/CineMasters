using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Accounts
{
    public static class IdentitySeedData
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager =
                app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();

            // Create an identity User if not exists
            IdentityUser user = await userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                user = new IdentityUser("Admin");
                user.Email = "jhel@avans.nl";
                await userManager.CreateAsync(user, "Testing123!");
            }
        }
    }
}
