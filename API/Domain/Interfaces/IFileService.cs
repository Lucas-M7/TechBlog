namespace API.Domain.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file);
}