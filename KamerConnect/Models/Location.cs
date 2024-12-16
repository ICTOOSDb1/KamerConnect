namespace KamerConnect.Models;

public class Location
{
    public int Range { get; set; }
    public Profile Profile { get; set; }
    public List<Coordinate> Geometry { get; set; }
    
    public Location(string name, Profile profile, List<Coordinate> geometry)
    {
        Profile = profile;
        Geometry = geometry;
    }

}

public enum Profile
{
    DrivingCar
}