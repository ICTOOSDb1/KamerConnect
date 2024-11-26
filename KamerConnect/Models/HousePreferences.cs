namespace KamerConnect.Models
{
    public class HousePreferences
    {
        public Guid? Id { get; set; }
        public double Budget { get; set; }
        public double SurfaceArea { get; set; }
        public HouseType Type { get; set; }
        public int Residents { get; set; }

        public HousePreferences(double budget, double surfaceArea, HouseType type, int residents, Guid? id)
        {
            Id = id;
            Budget = budget;
            SurfaceArea = surfaceArea;
            Type = type;
            Residents = residents;
        }
    }
}
