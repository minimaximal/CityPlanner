﻿using System.ComponentModel;
using System.Drawing;

namespace CityPlanner
{
    public class Agent
    {
        private Map _map;
        private List<Move> _parentMoves = new List<Move>();
        private List<Move> _moves = new List<Move>();
        private List<Move> _possibleMoves = new List<Move>();
        public bool NoMoreValidStreet;
        private static Move _firstPossibleMove = null!; //todo to be moved into data class
        private static Move _lastPossibleMove = null!; //todo to be moved into data class

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
            BasicSetup(map);
        }


        public Agent(Map map, Agent parent1, Agent parent2, double split)
        {
            _parentMoves = SelectMovesFromParents(parent1, parent2, split);
            BasicSetup(map);
        }

        private void BasicSetup(Map map)
        {
            _map = map;
            _firstPossibleMove = new Move(0, 0); //todo to be moved into data class
            _lastPossibleMove = new Move(_map.SizeX - 1, _map.SizeY - 1); //todo to be moved into data class
            NoMoreValidStreet = false;
            _possibleMoves.AddRange(_parentMoves);
            _possibleMoves.Sort();
            FillTheHoles();
        }

        private List<Move> SelectMovesFromParents(Agent parent1, Agent parent2, double split)//todo rename maybe generateMovesFromParents
        {
            List<Move> result = new List<Move>();
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
                result.Add(new Move(parent1._moves.ElementAt(i)));
            }

            //_moves.Count ==== shorterParentCount
            for (int i = result.Count; i < parent2._moves.Count; i++)
            {
                result.Add(new Move(parent2._moves.ElementAt(i)));
            }


            if (shorterParentCount == parent2._moves.Count) //parent2._moves.Count < parent1._moves.Count
            {
                for (int i = result.Count; i < parent1._moves.Count; i++)
                {
                    result.Add(new Move(parent1._moves.ElementAt(i)));
                }
            }

            return result;
        }

        private void FillTheHoles()
        {
            _possibleMoves.Insert(0, _firstPossibleMove);
            _possibleMoves.Add(_lastPossibleMove);

            for (int i = 0; i < _possibleMoves.Count - 1; i++)
            {
                if (_possibleMoves[i].IndexNumber() - _possibleMoves[i + 1].IndexNumber() < -1)
                {
                    //Ein loch ist da 
                    Move holeBeginning = _possibleMoves.ElementAt(i);
                    int y;
                    int holeOffset = 0;
                    do
                    {
                        int x = (holeBeginning.X + holeOffset + 1) % _map.SizeX;
                        y = x == 0 ? holeBeginning.Y + 1 : holeBeginning.Y;
                        if (_map.GetGridElement(x, y).GetGridType() == Data.GridType.Empty)
                        {
                            _possibleMoves.Insert(i + 1, new Move(x, y));
                            break;
                        }

                        holeOffset++;
                        //es muss abgebrochen werden soblad das andere ende von einem loch erreichtwurde
                        //(das elemet existert und wir machen weiter mit i)
                    } while (holeBeginning.X+ holeOffset+1 < _possibleMoves[i+1].X); 
                }
            }

            _possibleMoves.Remove(_firstPossibleMove);
            _possibleMoves.Remove(_lastPossibleMove);
            if (_possibleMoves[0].IndexNumber() != _firstPossibleMove.IndexNumber())
            {
                if (_map.GetGridElement(_firstPossibleMove)!.GetGridType() == Data.GridType.Empty)
                {
                    _possibleMoves.Insert(0, new Move(_firstPossibleMove));
                }
            }

            if (_possibleMoves[^1].IndexNumber() != _lastPossibleMove.IndexNumber())
            {
                if (_map.GetGridElement(_lastPossibleMove)!.GetGridType() == Data.GridType.Empty)
                {
                    _possibleMoves.Add(new Move(_lastPossibleMove));
                }
            }
        }

        public void MakeOneMove()
        {
            Random random = new Random();
            Move? move = null;
            if (_parentMoves.Count > 0)
            {
                do
                {
                    move = _parentMoves[0];
                    _parentMoves.RemoveAt(0);
                    //if parent move is bloced continue on with the next
                } while (_parentMoves.Count > 0 && !_possibleMoves.Contains(move));
            }

            if (move == null || random.NextDouble() < 0.02 ||
                (!_possibleMoves.Contains(move)) ||
                (IsIllegalStreet(move)))
            {
                if (_possibleMoves.Count > 0)
                {
                    move = GetRandomMove();
                }
                else
                {
                    NoMoreValidStreet = true; // boge work
                    //if there are no more valid moves at all we nead to stop the call of this funktion
                    // todo add AgentControlr: stopAgent(Agent)
                    return;
                }
            }

            RemoveFromPossibleMoves(move);


            _moves.Add(move);
            _map.AddMove(move);
        }

        private void RemoveFromPossibleMoves(Move move)
        {
            int index = _possibleMoves.IndexOf(move);
            // Goes and removes the move from posible list and checks sorted nighboirs for dupes 
            if (index > 0) //index- 1 >= 0 
            {
                Move ontTop = _possibleMoves[index - 1];
                if (ontTop.X == move.X &&
                    ontTop.Y == move.Y)
                {
                    _possibleMoves.Remove(ontTop);
                }
            }

            if (index + 1 < _possibleMoves.Count)
            {
                Move justBelow = _possibleMoves[index + 1];
                if (justBelow.X == move.X &&
                    justBelow.Y == move.Y)
                {
                    _possibleMoves.Remove(justBelow);
                }
            }

            _possibleMoves.Remove(move);
        }

        private bool IsIllegalStreet(Move move)
        {
            //if not a street -> false
            //if this move is a street ->  is legal street?

            if (move.GridType != Data.GridType.Street) return false;
            return !_map.ValidateStreet(move);
        }


        private Move GetRandomMove()
        {
            // get a tandom type to be placed
            Random random = new Random();

            // wenn straße dann spetzial fall

            Move move = _possibleMoves[random.Next(0, _possibleMoves.Count)];
            Data.GridType toBePlaced = Data.GridType.Housing;
        
            if (_map.ValidateStreet(move) && random.NextDouble() < 0.99 ) // staßen changese wenn sie möglich ist
            {
                toBePlaced = Data.GridType.Street;

            }
            else
            {
                double rand = random.NextDouble();
                if(rand < 0.5)
                {
                    toBePlaced = Data.GridType.Housing;
                }
                else if(rand< 0.8)
                {
                    toBePlaced = Data.GridType.Commercial;
                }
                else
                {
                    toBePlaced = Data.GridType.Industry;
                }
            }


       

            // fürge wo und wass zusammen und gib dies zurück
            move.GridType = toBePlaced;
            return move;
        }

        public void Display()
        {
            Console.WriteLine("Score:" + Score);
            Console.WriteLine("Population: " + _map.GetPeople());
            _map.NewDisplay();
        }

        public int GetMaxRemainingMoves()
        {
            return _possibleMoves.Count();
            // this is not true 
            // in 1 move  1or2 moves may be removed from _possibleMoves 
        }


        //DEBUG helper funktions
        public List<int> isinList(Move move)
        {
            List<int> hits = new List<int>();
            foreach (var m in _possibleMoves)
            {
                if (m.X == move.X && m.Y == move.Y)
                {
                    hits.Add(_possibleMoves.IndexOf(m));
                }
            }

            return hits;
        }
    }
}