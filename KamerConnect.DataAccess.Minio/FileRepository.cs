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

        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) ||
            string.IsNullOrEmpty(endpoint)
        )
        {
            Console.WriteLine("Minio environment variables are missing. Please check your .env file.");
        }

        _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
    }

    public async Task UploadFileAsync(string bucketName, string objectName, byte[] fileBytes, string contentType)
    {
        try
        {
            await CreateBucketIfNotExists(bucketName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithObjectSize(fileBytes.Length)
                    .WithStreamData(stream)
                    .WithContentType(contentType));
            }
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            throw;
        }
    }

    private async Task CreateBucketIfNotExists(string bucketName)
    {
        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName)
        );

        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName)
            );
        }
    }
}
