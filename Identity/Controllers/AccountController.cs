using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user=new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                };
              var result= await _userManager.CreateAsync(user,model.Password);
              if (result.Succeeded)
              {
                  return RedirectToAction("Index", "Home");
              }

              foreach (var error in result.Errors)
              {
                  ModelState.AddModelError("",error.Description);
              }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl=null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl=null)
        {
            if (ModelState.IsValid)
            {
               var result= await _signInManager.PasswordSignInAsync(
                   model.UserName,model.Password,model.RememberMe,true);
               if (result.Succeeded)
               {
                   if (string.IsNullOrEmpty(returnUrl))
                   {
                       return Redirect(returnUrl);
                   }
                   return RedirectToAction("Index", "Home");
               }

               if (result.IsLockedOut)
               {
                   ViewData["ErrorMessage"] = "اکانت شما به دلیل وارد کردن کلمه عبور اشتباه به مدت 5 دقیقه قفل شده است";
                   return View(model);
               }
               ModelState.AddModelError("","نام کاربری و یا کلمه عبور اشتباه وارد شده است");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
