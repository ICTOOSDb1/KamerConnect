using KamerConnect.Models;
using KamerConnect.Repositories;
using NetTopologySuite.Geometries;

namespace KamerConnect.Services;

public class HousePreferenceService
{
    private IHousePreferenceRepository _repository;
    private GeoLocationService _geoLocationService;
  

    public HousePreferenceService(IHousePreferenceRepository repository)
    {
        _repository = repository;
    }

    public void UpdateHousePreferences(HousePreferences housePreferences)
    {
        _repository.UpdateHousePreferences(housePreferences);
    }

    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        return _repository.CreateHousePreferences(housePreferences);
    }

    public void AddHousePreferences(Guid personId, Guid housePreferencesId)
    {
        _repository.AddHousePreferences(personId, housePreferencesId);
    }

    public HousePreferences? GetHousePreferences(Guid personId)
    {
        return _repository.GetHousePreferences(personId);
    }
}
