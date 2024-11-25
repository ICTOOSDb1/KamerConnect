using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task UploadFileAsync(string bucketName, string objectName, byte[] fileBytes, string contentType)
    {
        await _fileRepository.UploadFileAsync(bucketName, objectName, fileBytes, contentType);
    }
}
