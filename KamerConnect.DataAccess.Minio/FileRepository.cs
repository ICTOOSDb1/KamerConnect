using Amazon.S3;
using Amazon.S3.Model;
using KamerConnect.Repositories;

namespace KamerConnect.DataAccess.Minio;

public class FileRepository : IFileRepository
{
    private readonly AmazonS3Client _s3Client;

    public FileRepository()
    {
        var accessKey = Environment.GetEnvironmentVariable("MINIO_ROOT_USER");
        var secretKey = Environment.GetEnvironmentVariable("MINIO_ROOT_PASSWORD");
        var endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT");


        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Minio environment variables are missing. Please check your .env file.");
        }

        _s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
        {
            ServiceURL = $"http://{endpoint}",
            ForcePathStyle = true,
            UseHttp = true
        });
    }

    public async Task UploadFileAsync(string bucketName, string objectName, string filePath, string contentType)
    {
        try
        {
            await CreateBucketIfNotExists(bucketName);

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                FilePath = filePath,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(putRequest).ConfigureAwait(false);

            Console.WriteLine($"File '{objectName}' uploaded successfully to bucket '{bucketName}'.");
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error while uploading file: {ex.Message}");
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
            var bucketExists = await DoesBucketExistAsync(bucketName);

            if (!bucketExists)
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName
                });

                string bucketPolicy = @$"
                    {{
                        ""Version"": ""2012-10-17"",
                        ""Statement"": [
                            {{
                                ""Effect"": ""Allow"",
                                ""Principal"": ""*"",
                                ""Action"": ""s3:GetObject"",
                                ""Resource"": ""arn:aws:s3:::{bucketName}/*""
                            }}
                        ]
                    }}";

                var policyRequest = new PutBucketPolicyRequest
                {
                    BucketName = bucketName,
                    Policy = bucketPolicy
                };

                await _s3Client.PutBucketPolicyAsync(policyRequest);

                Console.WriteLine($"Bucket '{bucketName}' created successfully.");
            }
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error while checking/creating bucket: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> DoesBucketExistAsync(string bucketName)
    {
        try
        {
            var listResponse = await _s3Client.ListBucketsAsync();

            return listResponse.Buckets.Any(b => b.BucketName == bucketName);
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error while checking bucket existence: {ex.Message}");
            throw;
        }
    }
}
