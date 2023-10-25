namespace HFApp.WEB.Models.Domain.Dtos
{
    public class UsersDto : RequestDto
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
