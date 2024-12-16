using System;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IHousePreferenceRepository
{
    public void UpdateHousePreferences(HousePreferences housePreferences);
    Guid Create(HousePreferences housePreferences, Guid personId);
    HousePreferences? GetHousePreferences(Guid personId);

}
