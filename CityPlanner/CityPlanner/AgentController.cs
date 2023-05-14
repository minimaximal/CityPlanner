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
        
        public AgentController((int x, int y) mapSize, (int x, int y)[] startingPoints, int targetPopulation)
        {
            _mapSize = mapSize;
            _stratingPoints = startingPoints;
            _targetPopulation = targetPopulation;
        }

        public void ExecuteEvolutionStep() //didnt know better name, basically goes through one generation of agents
        {
            //todo decide on when to stop or rather: how often to check for populaiton; currently only one move per agent is made
            _agents.AsParallel().ForAll(agent => agent.MakeOneMove());
        }

        private void CreateNewAgents(int amount)
        {
            _agents.Clear();
            for (int i = 0; i < amount; i++)
            {
                Map map = CreateNewMap();
                _agents.Add(new Agent(map));
            }
        }

        private void CreateNewAgents(int amount, IEnumerable<Agent> precedingAgents)
        {
            //todo implement gene algorithm (so next gen is based on last gen)
        }

        private Map CreateNewMap()
        {
            //todo decide wheter to generate new map everytime or to implement IClonable in Map and only wholly generate once

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
