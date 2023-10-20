using HFApp.WEB.Models.Domain.Dtos;
using HFApp.WEB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HFApp.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
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
    }
}
