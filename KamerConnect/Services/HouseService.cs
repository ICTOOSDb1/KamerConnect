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

    public House Get(Guid id)
    {
        return _repository.Get(id);
    }

    public Guid Create(House house)
    {
        return _repository.Create(house);
    }

    public void Update(House house)
    {
        _repository.Update(house);
    }
}