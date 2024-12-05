using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class GeoLocationService
{
    private readonly IGeoLocationRepository _geoLocationRepository;

    public GeoLocationService(IGeoLocationRepository geoLocationRepository)
    {
        _geoLocationRepository = geoLocationRepository;
    }

    public async Task<string> GetGeoCode(string search)
    {
        return await _geoLocationRepository.GetGeoCode(search);
    }
}
