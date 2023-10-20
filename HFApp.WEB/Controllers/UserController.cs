using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Dtos;
using HFApp.WEB.Models.Domain.Entities;
using HFApp.WEB.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HFApp.WEB.Controllers
{
    [Authorize(Roles = "SuperUser,Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HFDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(HFDbContext context, UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> List()
        {
            var users  = await _userRepository.GetAll();
            var model = new UsersDto();
            foreach (var user in users)
            {
                model.Users.Add(new UserDto()
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email
                });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string ReturnUrl)
        {
            var model = new UserDto() { ReturnUrl = ReturnUrl };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserDto user)
        {
            var identityUser = new IdentityUser() { UserName = user.UserName, Email = user.Email };
            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(identityUser, "User");
                if (result2.Succeeded)
                {
                    _context.UserEntities.Add(new UserEntity()
                    {
                        CreatedAt = DateTime.UtcNow,
                        IdentityUserId = new Guid(identityUser.Id)
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("List","User");
        }
    }
}
