﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityPlanner
{
  
    
    public class AgentController
    {
        private List<Agent> _agents = new();
        private readonly int _agentAmount;
        private readonly (int x, int y) _mapSize;
        private readonly (int x, int y)[] _stratingPoints;
        private readonly int _targetPopulation;
        private readonly Map _Map;
        
        public AgentController((int x, int y) mapSize, (int x, int y)[] startingPoints, int targetPopulation, int agentAmount)
        {
            _mapSize = mapSize;
            _stratingPoints = startingPoints;
            _targetPopulation = targetPopulation;
            _agentAmount = agentAmount < 6 ? 6 : agentAmount;
            _Map = CreateNewMap();
        }

        public Agent ExecuteEvolutionStep() //didnt know better name, basically goes through one generation of agents
        {
            if(_agents.Count == 0)
            {
                CreateNewAgents(_agentAmount);
            }
            else
            {
                CreateNewAgents(_agentAmount, _agents);
            }

            int currentLargestPopulation = 0;
            List<Agent> finishedAgents = new();

            while(_agents.Count > 0)
            {
                int moveLimit = (_targetPopulation - currentLargestPopulation) / 200;
                moveLimit = moveLimit > _agents[0].getMaxRemainingMoves()
                    ? _agents[0].getMaxRemainingMoves()
                    : moveLimit;
                for(int moveNumber = 0; moveNumber < moveLimit ; moveNumber++)
                {
                    foreach (Agent agent in _agents)
                    {
                        agent.MakeOneMove();
                    }
                   // _agents.AsParallel().ForAll(agent => agent.MakeOneMove());
                }

                for (int i = 0;i< _agents.Count(); i++)
                {
                    if (_agents[i].noMoreValidStreet || _agents[i].Population > _targetPopulation)
                    {
                        finishedAgents.Add(_agents[i]);
                        _agents.Remove(_agents[i]);
                    }
                }
                
            }

            _agents = finishedAgents;

            return GetBestThreeAgents(finishedAgents)[0];
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
            List<Agent> bestThreeAgents = GetBestThreeAgents(precedingAgents);
            _agents.Clear();
            (int firstAgent, int secondAgent)[] combinations = new (int, int)[]{ (1, 2), (1, 3), (2, 3), (2, 1), (3, 1), (3, 2)};
            for (int i = 0; i < amount; i++)
            {
                Map map = (Map)_Map.Clone();
                _agents.Add(new Agent(map, bestThreeAgents[combinations[i % 6].firstAgent], bestThreeAgents[combinations[i % 6].secondAgent], (i+1)/(amount+1)));
            }
        }

        private List<Agent> GetBestThreeAgents(IEnumerable<Agent> precedingAgents)
        {
            return precedingAgents.OrderByDescending(agent => agent.Score).Take(3).ToList();
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
