using CineMasters.Areas.Accounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Helpers
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private UserManager<AppUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public RoleUsersTagHelper(UserManager<AppUser> usrMgr, RoleManager<IdentityRole> roleMgr)
        {
            UserManager = usrMgr;
            RoleManager = roleMgr;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await RoleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in UserManager.Users)
                {
                    if (user != null && await UserManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "Geen gebruikers met deze rol" : string.Join(", ", names));
        }
    }
}
