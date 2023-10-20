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
        private readonly HFDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(HFDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
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
                return RedirectToAction("Index","Home");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout(AccountDto account)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [Authorize( Roles = "SuperUser,Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(AccountDto account)
        {
            var identityUser = new IdentityUser() { UserName = account.UserName, Email = account.Email };
            var result = await _userManager.CreateAsync(identityUser,account.Password);
            if (result.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(identityUser, "User");
                if (result2.Succeeded)
                {
                    _context.UserEntities.Add(new UserEntity() { 
                        IdentityUserId = new Guid(identityUser.Id)
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult AccessDenied() {
            return View();
        }
    }
}
