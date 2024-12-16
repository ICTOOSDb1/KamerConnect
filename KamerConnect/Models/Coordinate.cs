namespace KamerConnect.Models;

public class Coordinate
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    
    public Coordinate(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}

