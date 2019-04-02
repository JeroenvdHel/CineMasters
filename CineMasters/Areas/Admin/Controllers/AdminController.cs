using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Accounts.Models;
using CineMasters.Areas.Accounts.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CineMasters.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public AdminController(UserManager<AppUser> usrMgr, RoleManager<IdentityRole> roleMgr)
        {
            UserManager = usrMgr;
            RoleManager = roleMgr;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult AllAccounts()
        {
            return View(UserManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllAccounts");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Geen gebruiker gevonden");
            }
            return View("AllAccount", UserManager.Users);
        }



        public IActionResult AllRoles()
        {
            return View(RoleManager.Roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Geen rollen gevonden");
            }
            return View("AllRoles", RoleManager.Roles);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();

            foreach (AppUser user in UserManager.Users)
            {
                var list = await UserManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await UserManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(AllRoles));
            }
            else
            {
                return await EditRole(model.RoleId);
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}