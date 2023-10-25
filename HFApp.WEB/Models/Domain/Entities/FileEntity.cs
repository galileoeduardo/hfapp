using System.Text.Json.Serialization;

namespace HFApp.WEB.Models.Domain.Entities
{
    public class FileEntity : BaseEntity
    {
        public Guid UID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MineTypesId { get; set; }

        [JsonIgnore]
        public MineTypesEntity MineTypes { get; set; } = null!;

        public int UserId { get; set; }

        [JsonIgnore]
        public UserEntity User { get; set; } = null!;

        public string? JsonData { get; set; }

    }
}
