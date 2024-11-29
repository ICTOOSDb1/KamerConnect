namespace KamerConnect.Models
{
    public class HousePreferences
    {


        public Guid Id { get; set; }
        public double MinBudget { get; set; }
        public double MaxBudget { get; set; }
        public double SurfaceArea { get; set; }
        public HouseType Type { get; set; }
        public int Residents { get; set; }
        public PreferenceChoice Smoking { get; set; }
        public PreferenceChoice Pet { get; set; }
        public PreferenceChoice Interior { get; set; }
        public PreferenceChoice Parking { get; set; }

        public HousePreferences(double minBudget, double maxBudget, double surfaceArea, HouseType type, int residents, PreferenceChoice smoking, PreferenceChoice pet, PreferenceChoice interior, PreferenceChoice parking, Guid id)
        {
            Id = id;
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            SurfaceArea = surfaceArea;
            Type = type;
            Residents = residents;
            Smoking = smoking;
            Pet = pet;
            Interior = interior;
            Parking = parking;
        }
    }
}

public enum PreferenceChoice
{
    Yes,
    No,
    No_Preferences
}
