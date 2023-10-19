using System.Text.Json.Serialization;

namespace HFApp.WEB.Models.Domain.Entities
{
    public class MineTypesEntity : BaseEntity
    {
        public string Extension { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        [JsonIgnore]
        public ICollection<FileEntity> File { get; set; }
    }
}
