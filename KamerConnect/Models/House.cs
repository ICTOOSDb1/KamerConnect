using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;
namespace KamerConnect.Models;

public class House
{
    public Guid Id { get; set; }
    public HouseType Type { get; set; }
    public double Price { get; set; }
    public string? Description { get; set; }
    public int Surface { get; set; }
    public int Residents { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public int HouseNumber { get; set; }
    public string HouseNumberAddition { get; set; }
    public Point HouseGeolocation { get; set; }
    public List<HouseImage> HouseImages { get; set; }

    public House(
        Guid id, HouseType type, double price, string? description, int surface, int residents,
        string city, string street, string postalCode, int houseNumber, string houseNumberAddition, Point houseGeolocation,
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
        HouseGeolocation = houseGeolocation;
        HouseImages = houseImages;
    }
}

public enum HouseType
{
    [Display(Name = "Appartement")]
    Apartment,
    [Display(Name = "Huis")]
    House,
    [Display(Name = "Studio")]
    Studio
}
