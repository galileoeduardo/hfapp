using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Dtos;
using HFApp.WEB.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HFApp.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly HFDbContext _context;
        private readonly DbSet<UserEntity> _dataset;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(HFDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _dataset = _context.Set<UserEntity>();
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDto account)
        {
            var result = await _signInManager.PasswordSignInAsync(account.UserName, account.Password, false, false);
            if (result.Succeeded)
            {
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


        [HttpPost]
        public async Task<IActionResult> Create(AccountDto account)
        {
            var identityUser = new IdentityUser() { UserName = account.UserName, Email = account.Email };
            var result = await _userManager.CreateAsync(identityUser,account.Password);
            if (result.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(identityUser, "User");
                if (result2.Succeeded)
                {
                    _dataset.Add(new UserEntity() { 
                        IdentityUserId = new Guid(identityUser.Id)
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return View();
        }
    }
}
