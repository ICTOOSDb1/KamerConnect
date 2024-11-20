namespace KamerConnect.Models.ConfigModels;

public class PasswordHashingConfig
{
    public int KeySize { get; set; }
    public int Iterations { get; set; }
    public string HashAlgorithm { get; set; }
}