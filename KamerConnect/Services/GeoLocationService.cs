using KamerConnect.Models;
using KamerConnect.Repositories;
using NetTopologySuite.Geometries;

namespace KamerConnect.Services;

public class GeoLocationService
{
    private readonly IGeoLocationRepository _geoLocationRepository;

    public GeoLocationService(IGeoLocationRepository geoLocationRepository)
    {
        _geoLocationRepository = geoLocationRepository;
    }

    public async Task<Point> GetGeoCode(string search)
    {
        return await _geoLocationRepository.GetGeoCode(search);
    }

    public async Task<Polygon> GetRangePolygon(int timeRange, Profile travelProfile, Point startLocation)
    {
        return await _geoLocationRepository.GetRangePolygon(timeRange, travelProfile, startLocation);
    }
}
