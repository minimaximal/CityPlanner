// @author: Paul Antoni, Sander Stella, Kevin Kern

// This program is only used for backend testing purposes as it is much faster than starting the frontend

using CityPlanner;

Map map = new Map(40, 40, 50000);
Move move = new Move(10, 10)
{
   GridType = Data.GridType.Street
};
map.AddMove(move);
Data.mutationChance = 0.001;
AppController appController = new AppController( map, 0,20);

Map? bestMap = null;

for (int j = 0; j < 1000000; j++)
{
   Map nextMap = appController.NextGeneration();

   if (j % 100 == 0)
   {
      Console.WriteLine("gen:" + j);

      nextMap.Display();
   }

   if (bestMap == null || bestMap.GetScore() < nextMap.GetScore())
   {

      bestMap = nextMap;

      Console.WriteLine("gen:" + j);

      nextMap.Display();
   }
}



