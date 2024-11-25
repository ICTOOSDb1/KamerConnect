using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.Models
{
    public class HousePreferences
    {
        public double Budget { get; set; }
        public double SurfaceArea { get; set; }
        public HouseType Type { get; set; }
        public int Residents { get; set; }

        public HousePreferences(double budget, double surfaceArea, HouseType type, int residents)
        {
            Budget = budget;
            SurfaceArea = surfaceArea;
            Type = type;
            Residents = residents;
        }
    }
    
    
}
