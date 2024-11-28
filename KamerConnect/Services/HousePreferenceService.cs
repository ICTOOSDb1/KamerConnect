using KamerConnect.Models;
using KamerConnect.Repositories;

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

    public Guid CreateHousePreferences(HousePreferences housePreferences)
    {
        return _repository.CreateHousePreferences(housePreferences);
    }

    public HousePreferences? GetHousePreferences(Guid personId)
    {
        return _repository.GetHousePreferences(personId);
    }
}
