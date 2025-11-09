namespace LibraryAPI.Models
{
    public class UploadDto
    {
        public IFormFile File { get; set; }
        public int UserId { get; set; }
    }
}
