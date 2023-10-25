namespace HFApp.WEB.Models.Domain.Dtos
{
    public class FileDto : RequestDto
    {
        public IFormFile file { get; set; }
        public int Id { get; set; }
        public Guid UID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public int MineTypesId { get; set; }
        public string? JsonData { get; internal set; }
    }
}
