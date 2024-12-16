using KamerConnect.Models;
using KamerConnect.Repositories;
using NetTopologySuite.Geometries;

namespace KamerConnect.Services;

public class HousePreferenceService
{
    private IHousePreferenceRepository _repository;

    public HousePreferenceService(IHousePreferenceRepository repository)
    {
        _repository = repository;
    }

    public void UpdateHousePreferences(HousePreferences housePreferences)
    {
        _repository.UpdateHousePreferences(housePreferences);
    }

    public Guid Create(HousePreferences housePreferences, Guid personId)
    {
        return _repository.Create(housePreferences, personId);
    }

    public HousePreferences? GetHousePreferences(Guid personId)
    {
        return _repository.GetHousePreferences(personId);
    }
}
