namespace CityPlanner;

public class AppController
{
   private AgentController agentController;

   public AppController(int population, int sizeX, int sizeY, int importQuota)
   {
      Data.ImportQuota = importQuota;
      Data.optimalIndustryAmount = (population * ((100 - Data.ImportQuota) / 100) / 2000);
      agentController = new AgentController((sizeX, sizeY), new (int, int)[] { (sizeX / 2, sizeY / 2) }, population, 10);
   }


   public Map nextGeneration()
   {
      Agent bestAgent = agentController.ExecuteEvolutionStep();
      return bestAgent.getMap();
   }

}