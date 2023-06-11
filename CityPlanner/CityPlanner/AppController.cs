// @author: Kevin Kern, Paul Antoni, Sander Stella

namespace CityPlanner;

public class AppController
{
    private readonly AgentController _agentController;
    private int _generation;

    public AppController(Map map, int importQuota, int numberAgents)
    {
        Data.ImportQuota = importQuota;
        Data.OptimalIndustryAmount = (int)(map.GetTargetPopulation() * ((100 - Data.ImportQuota) / 100.0) / 1750);
        _agentController = new AgentController(map, numberAgents);
        _generation = 0;
    }


    public Map NextGeneration()
    {
        var bestAgent = _agentController.ExecuteEvolutionStep();
        _generation++;
        return bestAgent.GetMap();
    }

    public int GetGeneration()
    {
        return _generation;
    }

}