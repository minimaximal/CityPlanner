// @author: Paul Antoni, Sander Stella, Kevin Kern

namespace CityPlanner
{
   public class AgentController
   {
      private readonly List<Agent> _agents = new();
      private readonly int _agentAmount;
      private readonly Map _defaultMap;
      private Agent? _bestOfAllTime;
      private readonly int _moveLimit;

      private bool _includeBestOfAllTime = true;


      public AgentController(Map map, int agentAmount)
      {

         Data.SizeX = map.SizeX;
         _agentAmount = agentAmount < 6 ? 6 : agentAmount;
         Data.InitialStreets = new List<(int, int)>();
         for (var x = 0; x < map.SizeX; x++)
         {
            for (var y = 0; y < map.SizeY; y++)
            {
               if (map.GetGridElement(x, y)!.GetGridType() == Data.GridType.Street)
               {
                  Data.InitialStreets.Add((x, y));
               }
            }
         }

         _defaultMap = map;
         _moveLimit = map.SizeX * map.SizeY - Data.InitialStreets.Count;
      }

      public Agent ExecuteEvolutionStep()
      {
         if (_agents.Count == 0)
         {
            CreateNewAgents(_agentAmount);
         }
         else
         {
            CreateNewAgents(_agentAmount, _agents);
         }

         _includeBestOfAllTime = true;

         _agents.AsParallel().ForAll(agent => agent.MakeNMoves(_moveLimit));
         _agents.AsParallel().ForAll(agent => agent.CalculateScore());

         var generationalBestAgent = _agents.OrderByDescending(agent => agent.Score).First();
         if (_bestOfAllTime != null && _bestOfAllTime.Score >= generationalBestAgent.Score)
             return generationalBestAgent;
         _bestOfAllTime = generationalBestAgent;
         _includeBestOfAllTime = false;
         return generationalBestAgent;
      }

      // Creates new Agents for first generation
      private void CreateNewAgents(int amount)
      {
         _agents.Clear();
         for (var i = 0; i < amount; i++)
         {
            var map = (Map)_defaultMap.Clone();
            _agents.Add(new Agent(map));
         }
      }

      // Creates new Agents with preceding Agents, used as parents in Agent.cs
      private void CreateNewAgents(int amount, IEnumerable<Agent> precedingAgents)
      {
         var bestThreeAgents = GetBestThreeAgents(precedingAgents);
         if (_includeBestOfAllTime)
            bestThreeAgents[2] = _bestOfAllTime!;
         _agents.Clear();
         (int firstAgent, int secondAgent)[] combinations = new (int, int)[]
             { (0, 1), (0, 2), (1, 2), (1, 0), (2, 0), (2, 1) };
         for (var i = 0; i < amount; i++)
         {
            var map = (Map)_defaultMap.Clone();
            _agents.Add(new Agent(map, bestThreeAgents[combinations[i % 6].firstAgent],
                bestThreeAgents[combinations[i % 6].secondAgent], (i + 1) / (double)(amount + 1)));
         }
      }

      private static List<Agent> GetBestThreeAgents(IEnumerable<Agent> precedingAgents)
      {
         return precedingAgents.OrderByDescending(agent => agent.Score).Take(3).ToList();
      }
   }
}