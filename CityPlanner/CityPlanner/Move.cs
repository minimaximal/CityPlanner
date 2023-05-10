using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
    class Move
    {
        public int X { get; }
        public int Y { get; }

        public Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
