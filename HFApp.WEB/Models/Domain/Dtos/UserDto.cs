using Microsoft.AspNetCore.Identity;

namespace HFApp.WEB.Models.Domain.Dtos
{
    public class UserDto : RequestDto
    {
        public string? Id { get; set; }
        public required string? UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; } = null!;
        public string? IdentityRoleName { get; set; }
    }
}
