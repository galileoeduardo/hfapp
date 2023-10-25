using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Dtos;
using HFApp.WEB.Models.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HFApp.WEB.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController( SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            var model = new AccountDto() { ReturnUrl = ReturnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDto account)
        {
            var result = await _signInManager.PasswordSignInAsync(account.UserName, account.Password, false, false);
            if (result.Succeeded)
            {
                if(!String.IsNullOrWhiteSpace(account.ReturnUrl))
                {
                    return RedirectToPage(account.ReturnUrl);
                }
                return RedirectToAction("Index","File");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout(AccountDto account)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult AccessDenied() {
            return View();
        }
    }
}
