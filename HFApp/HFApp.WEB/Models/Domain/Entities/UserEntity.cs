namespace HFApp.WEB.Models.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public required ICollection<RoleEntity> RoleEntities { get; set; }
    }
}
