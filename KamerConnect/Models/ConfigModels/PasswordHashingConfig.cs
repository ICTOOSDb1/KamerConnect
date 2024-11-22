namespace KamerConnect.Models.ConfigModels;

public class PasswordHashingConfig
{
    public PasswordHashingConfig(int keySize, int iterations, string hashAlgorithm)
    {
        KeySize = keySize;
        Iterations = iterations;
        HashAlgorithm = hashAlgorithm;
    }

    public int KeySize { get; set; }
    public int Iterations { get; set; }
    public string HashAlgorithm { get; set; }
}