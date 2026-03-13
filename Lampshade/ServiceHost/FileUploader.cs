using _0_Framework.Application;

namespace ServiceHost;

public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _environment;

    public FileUploader(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string Upload(IFormFile file, string path)
    {
        if (file == null) return "";
        var directoryPath = $"{_environment.WebRootPath}//ProductPictures//{path}";
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
        var filepath = $"{directoryPath}//{fileName}";
        using var output = File.Create(filepath);
        file.CopyTo(output);
        return $"{path}//{fileName}";
    }
}