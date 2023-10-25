using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Dtos;
using HFApp.WEB.Models.Domain.Entities;
using HFApp.WEB.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace HFApp.WEB.Controllers
{
    [Authorize(Roles = "SuperUser,Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HFDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(HFDbContext context, UserManager<IdentityUser> userManager, IUserRepository userRepository, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> List(UsersDto model)
        {
            var users  = await _userRepository.GetAll();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Users.Add(new UserDto()
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Password = "*******",
                    IdentityRoleName = string.Join(",", roles.ToArray()),
                    Email = (string.IsNullOrEmpty(user.Email)) ? string.Empty : user.Email,
                });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(UserDto model)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Insert(UserDto model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var child in ModelState.Root.Children)
                {
                    if (!child.Errors.Any()) continue;
                    model.Errors.Add(new ErrorDto()
                    {
                        Code = "ModelNotValid",
                        Description = child.Errors.FirstOrDefault().ErrorMessage
                    });
                }
                ViewData["IdentityRoleName"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View("Create", model);
            }

            var identityUser = new IdentityUser() { UserName = model.UserName, Email = model.Email };

            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {

                if (!ModelState.IsValid)
                {
                    foreach (var child in ModelState.Root.Children)
                    {
                        if (!child.Errors.Any()) continue;
                        model.Errors.Add(new ErrorDto()
                        {
                            Code = child.Errors.FirstOrDefault().Exception.Message,
                            Description = child.Errors.FirstOrDefault().ErrorMessage
                        });
                    }
                    ViewData["IdentityRoleName"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                    return View("Create", model);
                }
                var result2 = await _userManager.AddToRoleAsync(identityUser, model.IdentityRoleName);
                if (result2.Succeeded)
                {
                    _context.UserEntities.Add(new UserEntity()
                    {
                        CreatedAt = DateTime.UtcNow,
                        IdentityUserId = new Guid(identityUser.Id),
                        IdentityUserName = model.UserName
                    });
                    await _context.SaveChangesAsync();
                }
            } 
            else
            {
                if (result.Errors.Any())
                {
                    foreach (var erro in result.Errors)
                    {
                        model.Errors.Add(new ErrorDto()
                        {
                            Code = erro.Code,
                            Description = erro.Description
                        });
                    }
                    ViewData["IdentityRoleName"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name"); 
                    return View("Create",model);
                }
            }

            return RedirectToAction("List","User");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var idUser = await _userManager.FindByIdAsync(id.ToString());
            if (idUser is not null)
            {
                var result = _userManager.DeleteAsync(idUser);
                if (result is not null)
                {
                    var user = _context.UserEntities.SingleOrDefault(e => e.IdentityUserId.Equals(id));
                    if (user is not null)
                    {
                        _context.UserEntities.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("List", "User");
        }
    }
}
