using NetTopologySuite.Geometries;

namespace KamerConnect.Models;

public class Isochrone
{
    public Guid Id { get; }
    public int Range { get; set; }
    public Profile Profile { get; set; }
    public Polygon Geometry { get; set; }

    public Isochrone(Guid id, int range, Profile profile, Polygon geometry)
    {
        Id = id;
        Range = range;
        Profile = profile;
        Geometry = geometry;
    }
}

public enum Profile
{
    driving_car
}