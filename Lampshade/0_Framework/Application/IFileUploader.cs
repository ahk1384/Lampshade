using Microsoft.AspNetCore.Http;

namespace _0_Framework.Application;

public interface IFileUploader
{
    public string Upload(IFormFile file, string path);
}