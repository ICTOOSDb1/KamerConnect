using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task UploadFileAsync(string bucketName, string objectName, string filePath, string contentType)
    {
        await _fileRepository.UploadFileAsync(bucketName, objectName, filePath, contentType);
    }

    public string GetFilePath(string bucketName, string fileName)
    {
        var endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT");

        if (string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Minio environment variables are missing. Please check your .env file.");
        }

        return $"http://{endpoint}/{bucketName}/{fileName}";
    }
}
