using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace KamerConnect.Models
{
    public class HousePreferences
    {
        public Guid Id { get; set; }
        public double MinBudget { get; set; }
        public double MaxBudget { get; set; }
        public string City { get; set; }
        public Point? CityGeolocation { get; set; }
        public double SurfaceArea { get; set; }
        public HouseType Type { get; set; }
        public int Residents { get; set; }
        public PreferenceChoice Smoking { get; set; }
        public PreferenceChoice Pet { get; set; }
        public PreferenceChoice Interior { get; set; }
        public PreferenceChoice Parking { get; set; }
        public SearchArea SearchArea { get; set; }

        public HousePreferences(double minBudget, double maxBudget, string city, Point? cityGeolocation, double surfaceArea, HouseType type, int residents, PreferenceChoice smoking, PreferenceChoice pet, PreferenceChoice interior, PreferenceChoice parking, Guid id, SearchArea searchArea)
        {
            Id = id;
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            City = city;
            CityGeolocation = cityGeolocation;
            SurfaceArea = surfaceArea;
            Type = type;
            Residents = residents;
            Smoking = smoking;
            Pet = pet;
            Interior = interior;
            Parking = parking;
            SearchArea = searchArea;
        }
    }
}

public enum PreferenceChoice
{
    [Display(Name = "Ja")]
    Yes,
    [Display(Name = "Nee")]
    No,
    [Display(Name = "Geen voorkeur")]
    No_preference
}
