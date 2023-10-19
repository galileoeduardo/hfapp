using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace HFApp.WEB.Models.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public Guid IdentityUserId { get; set; }
        [JsonIgnore]
        public ICollection<FileEntity> Files { get; set; }

    }
}
