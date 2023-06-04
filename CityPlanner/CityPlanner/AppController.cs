namespace CityPlanner;

public class AppController
{
   private AgentController agentController;
   private int generation;

   public AppController(int population, int sizeX, int sizeY, int importQuota)
   {
      Data.ImportQuota = importQuota;
      Data.optimalIndustryAmount = (population * ((100 - Data.ImportQuota) / 100) / 1500);
      agentController = new AgentController((sizeX, sizeY), new (int, int)[] { (sizeX / 2, sizeY / 2) }, population, 10);
      generation = 0;
   }


   public Map nextGeneration()
   {
      Agent bestAgent = agentController.ExecuteEvolutionStep();
      generation++;
      return bestAgent.getMap();
   }

   public int getGeneration()
   {
      return generation;
   }

}