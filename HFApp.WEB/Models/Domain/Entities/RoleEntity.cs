namespace HFApp.WEB.Models.Domain.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public required ICollection<UserEntity> UserEntities { get; set; }
    }
}
