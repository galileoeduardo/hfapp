using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace HFApp.WEB.Models.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public RoleEntity Role { get; set; } = null!;

    }
}
