namespace API_TEST.Helpers
{
    public class FileHelper
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string uploadDirectory)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4", ".avi", ".mov" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return null;
            }

            string subDirectory = fileExtension switch
            {
                ".jpg" or ".jpeg" or ".png" => "images",
                ".mp4" or ".avi" or ".mov" => "videos",
                _ => throw new InvalidOperationException("Unsupported file type")
            };

            string relativePath = $"/{subDirectory}/";
            string newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow.Ticks}{fileExtension}";
            string filePath = Path.Combine(uploadDirectory, subDirectory, newFileName);

            Directory.CreateDirectory(Path.Combine(uploadDirectory, subDirectory));

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return relativePath + newFileName;
        }
    }
}
