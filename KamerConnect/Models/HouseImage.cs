namespace KamerConnect.Models;

public class HouseImage
{
    public string Path { get; set; }
    public string Bucket { get; set; }

    public HouseImage(string path, string bucket)
    {
        Path = path;
        Bucket = bucket;
    }
}
