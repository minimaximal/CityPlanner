using System.ComponentModel;
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
            _map = map;
            _firstPossibleMove = new Move(0, 0); //todo to be moved into data class
            _lastPossibleMove = new Move(_map.SizeX - 1, _map.SizeY - 1); //todo to be moved into data class
            NoMoreValidStreet = false;
            _possibleMoves.AddRange(_parentMoves);
            _possibleMoves.Sort();
            FillTheHoles();
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
                _parentMoves.Add(new Move( parent1._moves.ElementAt(i)));
            }

            //_moves.Count ==== shorterParentCount
            for (int i = _moves.Count; i < parent2._moves.Count; i++)
            {
                _parentMoves.Add(new Move( parent2._moves.ElementAt(i)));
            }

            if (shorterParentCount == parent2._moves.Count) //parent2._moves.Count < parent1._moves.Count
            {
                for (int i = _moves.Count; i < parent1._moves.Count; i++)
                {
                    _parentMoves.Add(new Move( parent1._moves.ElementAt(i)));
                }
            }
        }
        private void FillTheHoles()
        {
            
            //todo bug letzte zeile wird nie befült auser das letzt letze element 
            
            _possibleMoves.Add(_firstPossibleMove);
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
                        y = x == 0 ? holeBeginning.Y+1 : holeBeginning.Y;
                        if (_map.GetGridElement(x, y).GetGridType() == Data.GridType.Empty)
                        {
                            _possibleMoves.Insert(i + 1, new Move(x, y));
                            break;
                        }

                        holeOffset++;
                    } while (y <= _map.SizeY);/// todo fix idee anstelle von < ein <= nutzen //muss geprüft werden ob das sin macht
                }
            }

            _possibleMoves.Remove(_firstPossibleMove);
            _possibleMoves.Remove(_lastPossibleMove);
            if (_possibleMoves[0].IndexNumber() != _firstPossibleMove.IndexNumber())
            {
                if (_map.GetGridElement(_firstPossibleMove).GetGridType() == Data.GridType.Empty)
                {
                    _possibleMoves.Insert(0, new Move(_firstPossibleMove));
                }
            }

            if (_possibleMoves[^1].IndexNumber() != _lastPossibleMove.IndexNumber())
            {
                if (_map.GetGridElement(_lastPossibleMove).GetGridType() == Data.GridType.Empty)
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
                move =  _parentMoves[0];
                _parentMoves.RemoveAt(0);
            }

            if (move == null || random.NextDouble() < 0.015 ||
                (!IsLegalMove(move)) ||
                (!IsLegalStreet(move)))
            {
                move = GetRandomMove();
            }

            RemovePossibleMoves(move);
            _moves.Add(move);
            _map.AddMove(move);
        }

        private void RemovePossibleMoves(Move move)
        {
            int index = _possibleMoves.IndexOf(move);
            // Goes and removes the move from posible list and checks sorted nighboirs for dupes 
            if (index > 0)  //index- 1 >= 0 
            {
                Move ontTop = _possibleMoves[index- 1];
                if (ontTop.X == move.X &&
                    ontTop.Y == move.Y)
                {
                    _possibleMoves.Remove(ontTop);
                }
            }

            if (index+ 1 < _possibleMoves.Count)
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
        
        private bool IsLegalMove(Move move)
        {
            return _possibleMoves.Contains(move);
        }

        private bool IsLegalStreet(Move move)
        {
            //if not a street -> false
            //if this move is a street ->  is legal street?
            return (move.GridType == Data.GridType.Street && _map.ValidateStreet(move));
        }


        private Move GetRandomMove()
        {
            // get a tandom type to be placed
            Random random = new Random();
            Data.GridType toBePlaced;
            do
            {
                toBePlaced = (Data.GridType)Enum.GetValues(typeof(Data.GridType))
                    .GetValue(random.Next(Data.GridTypeAmount));
            } while (NoMoreValidStreet && toBePlaced == Data.GridType.Street);

            // wenn straße dann spetzial fall 

            Move move;
            if (toBePlaced == Data.GridType.Street)
            {
                move = GetRandomStreet();
            }
            else
            {
                move = _possibleMoves[random.Next(0, _possibleMoves.Count)];
            }

            // fürge wo und wass zusammen und gib dies zurück
            move.GridType = toBePlaced;
            return move;
        }

        Move GetRandomStreet()
        {
            Random random = new Random();

            List<Move> limitedMoves = new List<Move>();
            foreach (var possibleStreet in _possibleMoves)
            {
                if (_map.GetGridElement(possibleStreet).IsValidStreet())
                {
                    limitedMoves.Add(possibleStreet);
                }
            }

            if (limitedMoves.Count == 0)
            {
                NoMoreValidStreet = true;
                return GetRandomMove(); //todo maybe no recursion ??
            }

            int rand = random.Next(0, limitedMoves.Count);
            return limitedMoves[rand];
        }

        public void Display()
        {
            Console.Write("Score:" + Score);
            _map.NewDisplay();
        }

        public int GetMaxRemainingMoves()
        {
            return _possibleMoves.Count();
        }
    }
}