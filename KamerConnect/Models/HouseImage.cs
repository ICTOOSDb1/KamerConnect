namespace KamerConnect.Models;

public class HouseImage
{
    public string Path { get; }
    public string Bucket { get; }
    public string FullPath { get => $"http://{Environment.GetEnvironmentVariable("MINIO_ENDPOINT")}/{Bucket}/{Path}"; }

    public HouseImage(string path, string bucket)
    {
        Path = path;
        Bucket = bucket;
    }
}
