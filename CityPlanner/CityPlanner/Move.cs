namespace CityPlanner
{
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

        public Move(Move oldMove)
        {
            X = oldMove.X;
            Y = oldMove.Y;
            GridType = oldMove.GridType;
        }
        
        
    }
}
