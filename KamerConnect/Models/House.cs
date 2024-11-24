namespace KamerConnect.Models;

public class House
{
    public string? Id { get; set; }
    public HouseType Type { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Surface { get; set; }
    public int Residents { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public int HouseNumber { get; set; }
    public string HouseNumberAddition { get; set; }
    public List<HouseImage> HouseImages { get; set; }

    public House(
        string? id, HouseType type, double price, string description, int surface, int residents,
        string city, string street, string postalCode, int houseNumber, string houseNumberAddition,
        List<HouseImage> houseImages)
    {
        Id = id;
        Type = type;
        Price = price;
        Description = description;
        Surface = surface;
        Residents = residents;
        City = city;
        Street = street;
        PostalCode = postalCode;
        HouseNumber = houseNumber;
        HouseNumberAddition = houseNumberAddition;
        HouseImages = houseImages;
    }
}

public enum HouseType
{
    Apartment,
    House,
    Studio
}

