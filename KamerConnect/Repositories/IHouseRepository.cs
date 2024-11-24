using System;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IHouseRepository
{
    public string CreateHouse(House house);
}
