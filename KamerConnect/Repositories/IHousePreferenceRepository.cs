using System;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IHousePreferenceRepository
{
    public void UpdateHousePreferences(HousePreferences housePreferences);
    Guid CreateHousePreferences(HousePreferences housePreferences);
    HousePreferences? GetHousePreferences(Guid personId);
}
