namespace CityPlanner;

public class AppController
{
   private AgentController _agentController;
   private int _generation;

   public AppController(int population, Map map, int importQuota)
   {
      Data.ImportQuota = importQuota;
      Data.OptimalIndustryAmount = (population * ((100 - Data.ImportQuota) / 100) / 1250);
      _agentController = new AgentController(map, 20);
      _generation = 0;
   }


   public Map NextGeneration()
   {
      Agent bestAgent = _agentController.ExecuteEvolutionStep();
      _generation++;
      return bestAgent.GetMap();
   }

   public int GetGeneration()
   {
      return _generation;
   }

}