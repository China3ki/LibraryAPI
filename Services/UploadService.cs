namespace LibraryAPI.Services
{
    public enum ImageType { Cover, Author, User }
    public class UploadService
    {
        public async Task<string> UploadCover(string filename, IFormFile file, ImageType imageType)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            string directory = imageType switch
            {
                ImageType.Cover => "Covers",
                ImageType.User => "Users",
                ImageType.Author => "Authors",
                _ => throw new NotImplementedException()
            };
            var path = Path.Combine($"{Directory.GetCurrentDirectory()}", "Images", directory, $"{filename}.{fileExtension}");
            using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);
            return $"{path}";
        }
    }
}
