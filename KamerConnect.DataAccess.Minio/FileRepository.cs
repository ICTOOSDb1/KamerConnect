using KamerConnect.Repositories;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace KamerConnect.DataAccess.Minio;

public class FileRepository : IFileRepository
{
    private readonly IMinioClient _minioClient;

    public FileRepository()
    {
        var accessKey = Environment.GetEnvironmentVariable("MINIO_KEY");
        var secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET");
        var endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT");

        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Minio environment variables are missing. Please check your .env file.");
        }

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task UploadFileAsync(string bucketName, string objectName, string filePath, string contentType)
    {
        try
        {
            await CreateBucketIfNotExists(bucketName);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithFileName(filePath)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            Console.WriteLine($"File '{objectName}' uploaded successfully to bucket '{bucketName}'.");
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Minio error while uploading file: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while uploading file: {ex.Message}");
            throw;
        }
    }

    private async Task CreateBucketIfNotExists(string bucketName)
    {
        try
        {
            var bucketExists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName)
            );

            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(bucketName)
                );
                Console.WriteLine($"Bucket '{bucketName}' created successfully.");
            }
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Minio error while checking/creating bucket: {ex.Message}");
            throw;
        }
    }
}
