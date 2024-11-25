namespace KamerConnect.Models
{
    public class HousePreferences
    {
        public string Budget { get; set; }
        public string SurfaceArea { get; set; }
        public HouseType? Type { get; set; }
        public string Residents { get; set; }
        public HousePreferences(string budget, string surfaceArea, HouseType? type, string residents)
        {
            Budget = budget;
            SurfaceArea = surfaceArea;
            Type = type;
            Residents = residents;
        }
    }
}