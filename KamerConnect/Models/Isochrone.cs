using NetTopologySuite.Geometries;

namespace KamerConnect.Models;

public class Isochrone
{
    public int Range { get; set; }
    public Profile Profile { get; set; }
    public Polygon Geometry { get; set; }
    
    public Isochrone(int range, Profile profile, Polygon geometry)
    {
        Range = range;
        Profile = profile;
        Geometry = geometry;
    }

}

public enum Profile
{
    Driving_Car
}