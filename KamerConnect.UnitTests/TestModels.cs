using System;
using KamerConnect.Models;

namespace KamerConnect.UnitTests;

public class TestModels
{
    public static House HouseModel = new House(Guid.NewGuid(), HouseType.House, 900.50, "Test description", 15, 3, "Stad", "Straatnaam", "1234AB", 12, "A", new List<HouseImage>());
}
