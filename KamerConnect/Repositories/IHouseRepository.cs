using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IHouseRepository
{
    void Update(House house);
    Guid Create(House house, Guid personId);
    House? GetByPersonId(Guid personId);
    List<House> GetByPreferences(HousePreferences housePreferences);
    House? Get(Guid id);
}
