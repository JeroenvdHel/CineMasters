using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CineMasters.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> UserManager;
        private SignInManager<AppUser> SignInManager;

        public AccountController(UserManager<AppUser> usrMgr, SignInManager<AppUser> signInMgr)
        {
            UserManager = usrMgr;
            SignInManager = signInMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await SignInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await SignInManager.PasswordSignInAsync(user, details.Password, false, false);

                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                    "Ongeldig e-mailadres of wachtwoord");
            }
            return View(details);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("AllAccounts", "Admin");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditAccount(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            EditModel model = new EditModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(EditModel model)
        {
            AppUser user = await UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.UserName = model.Name;
                user.Email = model.Email;
                await UserManager.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}