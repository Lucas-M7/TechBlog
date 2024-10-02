using API.Domain.Interfaces;

namespace API.Services;

public class FileService : IFileService
{
    private readonly string _uploadFolder;

    public FileService(IWebHostEnvironment env)
    {
        _uploadFolder = Path.Combine(env.WebRootPath, "uploads");
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        var filePath = Path.Combine(_uploadFolder, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("uploads", file.FileName);
    }
}