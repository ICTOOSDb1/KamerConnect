using System;
using KamerConnect.Models;

namespace KamerConnect.Repositories;

public interface IGeoLocationRepository
{
    Task<string> GetGeoCode(string search);
}
