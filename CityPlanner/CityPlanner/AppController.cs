﻿namespace CityPlanner;

public class AppController
{
   private AgentController agentController;
   private int generation;

   public AppController(int population, int sizeX, int sizeY, int importQuota)
   {
      Data.ImportQuota = importQuota;
      Data.OptimalIndustryAmount = (population * ((100 - Data.ImportQuota) / 100) / 1250);
      agentController = new AgentController((sizeX, sizeY), new (int, int)[] { (sizeX / 2, sizeY / 2) }, population, 20);
      generation = 0;
   }


   public Map nextGeneration()
   {
      Agent bestAgent = agentController.ExecuteEvolutionStep();
      generation++;
      return bestAgent.GetMap();
   }

   public int getGeneration()
   {
      return generation;
   }

}