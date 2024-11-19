namespace KamerConnect.Repositories;

public interface IFileRepository
{
    Task UploadFileAsync(string bucketName, string objectName, byte[] fileBytes, string contentType);
}
