using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IHouseRepository
{
    void Update(House house);
    House Get(Guid id);
    Guid Create(House house);
}
