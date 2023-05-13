using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    // könnte einfach nur eine auf ein objekt alten und das wäre dann eine move liste 
    public class Move
    {
        public Data.GridType GridType;
        public int X { get; }
        public int Y { get; }

        public Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
