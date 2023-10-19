using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace HFApp.WEB.Models.Domain.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        [JsonIgnore]
        public ICollection<UserEntity> User { get; set; }
    }
}
