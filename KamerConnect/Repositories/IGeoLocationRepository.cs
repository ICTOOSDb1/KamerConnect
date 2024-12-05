using System;
using KamerConnect.Models;
using NetTopologySuite.Geometries;

namespace KamerConnect.Repositories;

public interface IGeoLocationRepository
{
    Task<Point> GetGeoCode(string search);
}
