namespace LibraryAPI.Services
{
    public enum ImageType { Cover, Author, User }
    public class UploadService
    {
        public async Task<string> Upload(string filename, IFormFile file, ImageType imageType)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            string directory = imageType switch
            {
                ImageType.Cover => "Covers",
                ImageType.User => "Users",
                ImageType.Author => "Authors",
                _ => throw new NotImplementedException()
            };
            var path = Path.Combine("wwwroot", "Images", directory, $"{filename}.{fileExtension}");
            using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);
            return Path.Combine("https://localhost:7051/", "Images", directory, $"{filename}.{fileExtension}"); ;
        }
    }
}
