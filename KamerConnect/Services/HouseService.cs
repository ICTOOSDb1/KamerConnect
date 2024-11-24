using KamerConnect.Models;
using KamerConnect.Repositories;

namespace KamerConnect.Services;

public class HouseService
{
    private IHouseRepository _repository;

    public HouseService(IHouseRepository repository)
    {
        _repository = repository;
    }

    public string Create(House house)
    {
        return _repository.CreateHouse(house);
    }
}