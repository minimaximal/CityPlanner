using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using CityPlanner.Grid;

namespace CityPlanner
{
    public class Agent
    {
        private Map _map;
        private List<Move> _moves = new List<Move>();
        private int _moveCounter = 0;
        private List<Move> _emptyMoves = new List<Move>();
        public bool noMoreValidStreet;

        public int Population
        {
            get => _map.GetPeople();
        }

        public int Score
        {
            get => _map.CalculateScore();
        }


        public Agent(Map map)
        {
            noMoreValidStreet = false;
            _map = map;
            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    if (map.GetGridElement(x, y).GetGridType() == Data.GridType.Empty)
                    {
                        _emptyMoves.Add(new Move(x, y));
                    }
                }
            }
        }

        public Agent(Map map, Agent parent1, Agent parent2, double split) : this(map)
        {
            if (split >= 1)
            {
                split = 1;
            }

            int shorterParentCount = parent1._moves.Count;

            if (parent2._moves.Count < parent1._moves.Count)
            {
                shorterParentCount = parent2._moves.Count;
            }

            for (int i = 0; i < shorterParentCount * split; i++)
            {
                _moves.Add(parent1._moves.ElementAt(i));
            }

            //_moves.Count ==== shorterParentCount
            for (int i = _moves.Count; i < parent2._moves.Count; i++)
            {
                _moves.Add(parent2._moves.ElementAt(i));
            }

            if (shorterParentCount == parent2._moves.Count) //parent2._moves.Count < parent1._moves.Count
            {
                for (int i = _moves.Count; i < parent1._moves.Count; i++)
                {
                    _moves.Add(parent1._moves.ElementAt(i));
                }
            }
        }

        public void MakeOneMove()
        {
            Random random = new Random();
            Move move = null;
            if (_moves.Count > _moveCounter)
            {
                move = _moves.ElementAt(_moveCounter);
            }

            if (move == null || random.NextDouble() < 0.015 ||
                (!isLegalMove(move)) ||
                (!isLegalStreet(move)))
            {
                move = getRandomMove();
                _moves.Add(move);
            }

            _map.AddMove(move);
            _moveCounter++;
        }

        private bool isLegalMove(Move move)
        {
            return _map.GetGridElement(move).GetGridType() == Data.GridType.Empty;
        }

        private bool isLegalStreet(Move move)
        {
            return !(move.GridType == Data.GridType.Street && !_map.validateStreet(move));
        }


        Move getRandomMove()
        {
            // copy pasted from https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
            Array values = Enum.GetValues(typeof(Data.GridType));
            Random random = new Random();
            Data.GridType toBePlaced;
            do
            {
                toBePlaced = (Data.GridType)values.GetValue(random.Next(values.Length));
            } while (noMoreValidStreet && toBePlaced == Data.GridType.Street);

            //end copy

            int rand = 0;
            Move move ;
            if (!noMoreValidStreet && toBePlaced == Data.GridType.Street)
            {
                move = getRandomStreet();
            }
            else
            {
                rand = random.Next(0, _emptyMoves.Count);
                move = _emptyMoves[rand];
            }

            
            move.GridType = toBePlaced;
            _emptyMoves.Remove(move);
            return move;
        }

        Move getRandomStreet()
        {
            Random random = new Random();

            List<Move> limitedMoves = new List<Move>();
            foreach (var possibleStreet in _emptyMoves)
            {
                if (_map.GetGridElement(possibleStreet).IsValidStreet())
                {
                    limitedMoves.Add(possibleStreet);
                }
            }

            if (limitedMoves.Count == 0)
            {
                noMoreValidStreet = true;
                return getRandomMove(); //todo maybe no recursion ??
            }

            int rand = random.Next(0, limitedMoves.Count);
            return limitedMoves[rand];
        }

        public void Display()
        {
            _map.Display();
        }

        public int getMaxRemainingMoves()
        {
            return _emptyMoves.Count();
        }
    }
}