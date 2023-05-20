namespace CityPlanner
{
    public class Move
    {
        public Data.GridType GridType;
        public readonly int X;
        public readonly int Y;

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
