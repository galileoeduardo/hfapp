namespace HFApp.WEB.Models.Domain.Dtos
{
    public class RequestDto
    {
        public string? ReturnUrl { get; set; }
        public List<ErrorDto> Errors { get; set; }  = new List<ErrorDto>();
    }
}
