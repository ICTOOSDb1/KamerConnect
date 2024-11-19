namespace KamerConnect.Models;

public class House
{
    public string? Id { get; set; }
    public HouseType Type { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Surface { get; set; }
    public int Residents { get; set; }
    public List<HouseImage> HouseImages { get; set; }

    public House(
        string? id, HouseType type, double price, string description, int surface, int residents,
        List<HouseImage> houseImages)
    {
        Id = id;
        Type = type;
        Price = price;
        Description = description;
        Surface = surface;
        Residents = residents;
        HouseImages = houseImages;
    }
}

public enum HouseType
{
    Apartment,
    House,
    Studio
}

