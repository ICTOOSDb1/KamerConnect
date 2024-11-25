using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.Models
{
    public class HousePreferences
    {
        public string Budget;
        public string SurfaceArea;
        public HouseType Type;

        public HousePreferences(string budget, string surfaceArea, HouseType type)
        {
            Budget = budget;
            SurfaceArea = surfaceArea;
            Type = type;
        }
    }
}
