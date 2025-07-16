using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;

namespace Infrastructure.Services
{
    public class MediaUploadService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public MediaUploadService(IHttpContextAccessor http)
        {
            httpContextAccessor = http;
        }

        public async Task<string> UploadImage(IFormFile image, string imageName)
        {
            var originalExtension = Path.GetExtension(image.FileName).ToLower();
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var sanitizedImageName = string.Join("_", imageName.Split(Path.GetInvalidFileNameChars()));
            var fileNameWithoutExt = $"{sanitizedImageName}_Tahfez-Quran";
            var webpFileName = fileNameWithoutExt + ".webp";
            var webpFilePath = Path.Combine(uploadsFolder, webpFileName);
            using var webPImage = await Image.LoadAsync(image.OpenReadStream());
            await webPImage.SaveAsync(webpFilePath, new WebpEncoder { Quality = 75 });

            var request = httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/uploads/{webpFileName}";
        }
    }
}
