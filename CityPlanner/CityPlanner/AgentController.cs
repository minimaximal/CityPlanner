using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
  
    
    public class AgentController
    {
        private List<Agent> _agents = new();
        private readonly (int x, int y) _mapSize;
        private readonly (int x, int y)[] _stratingPoints;
        private readonly int _targetPopulation;
        private readonly Map _Map;
        
        public AgentController((int x, int y) mapSize, (int x, int y)[] startingPoints, int targetPopulation)
        {
            _mapSize = mapSize;
            _stratingPoints = startingPoints;
            _targetPopulation = targetPopulation;
            _Map = CreateNewMap();
        }

        public void ExecuteEvolutionStep() //didnt know better name, basically goes through one generation of agents
        {
            int currentLargestPopulation = 0;
            while(currentLargestPopulation < _targetPopulation)
            {
                for(int moveNumber = 0; moveNumber < (_targetPopulation - currentLargestPopulation) / 200; moveNumber++)
                {
                    _agents.AsParallel().ForAll(agent => agent.MakeOneMove());
                }

                currentLargestPopulation = _agents.OrderByDescending(agent => agent.Population).FirstOrDefault().Population;
            }
            
        }

        private void CreateNewAgents(int amount)
        {
            _agents.Clear();
            for (int i = 0; i < amount; i++)
            {
                Map map = (Map)_Map.Clone();
                _agents.Add(new Agent(map));
            }
        }

        private void CreateNewAgents(int amount, IEnumerable<Agent> precedingAgents)
        {
            //todo implement gene algorithm (so next gen is based on last gen)
        }

        private Map CreateNewMap()
        {
            Map map = new Map(_mapSize.x, _mapSize.y);
            foreach ((int x, int y) in _stratingPoints)
            {
                Move move = new Move(x, y)
                {
                    GridType = Data.GridType.Street
                };
                map.AddMove(move);
            }

            return map;
        }
    }
}
