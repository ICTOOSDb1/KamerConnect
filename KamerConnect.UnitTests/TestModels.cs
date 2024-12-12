using System;
using KamerConnect.Models;
using NetTopologySuite.Geometries;

namespace KamerConnect.UnitTests;

public class TestModels
{
    public static House HouseModel = new House(Guid.NewGuid(), HouseType.House, 900.50, "Test description", 15, 3, "Stad", "Straatnaam", "1234AB", 12, "A", new Point(4.23, 4.24556), new List<HouseImage>(), true, PreferenceChoice.Yes, PreferenceChoice.Yes, PreferenceChoice.Yes, PreferenceChoice.Yes);
}
