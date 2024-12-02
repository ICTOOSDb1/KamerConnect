namespace KamerConnect.Repositories;

public interface IFileRepository
{
    Task UploadFileAsync(string bucketName, string objectName, string filePath, string contentType);
}
