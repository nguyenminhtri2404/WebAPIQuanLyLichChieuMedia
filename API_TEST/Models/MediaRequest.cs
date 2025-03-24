namespace API_TEST.Models
{
    public class MediaRequest
    {
        public string Title { get; set; }
        public IFormFile FilePath { get; set; }
        public string MediaType { get; set; }
    }
}
