using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HFApp.WEB.Models.Domain.Dtos
{
    public class AccountDto : RequestDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
