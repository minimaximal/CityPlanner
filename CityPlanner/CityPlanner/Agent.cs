namespace CityPlanner
{
    public class Agent
    {
        private Map _map;
        private List<Move> _parentMoves = new List<Move>();
        private List<Move> _moves = new List<Move>();
        private List<Move> _possibleMoves = new List<Move>();
        public bool NoMoreValidMoves;
        private static Move _firstPossibleMove = null!; //todo to be moved into data class
        private static Move _lastPossibleMove = null!; //todo to be moved into data class

        private static List<(int x, int y)> _listStartingStreets = new List<(int x, int y)>();


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
            NoMoreValidMoves = false;
            _possibleMoves.AddRange(_parentMoves);
            _possibleMoves = _possibleMoves.OrderBy(move => move.IndexNumber()).ToList();
       
            FillGapsInMovesList();
            _possibleMoves = _possibleMoves.OrderBy(move => move.DistanceToCenter(_listStartingStreets)).ToList();
        }

       
        private List<Move> SelectMovesFromParents(Agent parent1, Agent parent2, double split)
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

        private void FillGapsInMovesList()
        {
            _possibleMoves.Insert(0, _firstPossibleMove);
            _possibleMoves.Add(_lastPossibleMove);

            for (int i = 0; i < _possibleMoves.Count - 1; i++)
            {
                if (_possibleMoves[i].IndexNumber() - _possibleMoves[i + 1].IndexNumber() < -1)
                {
                    FillGapAt(i);
                }
            }

            _possibleMoves.Remove(_firstPossibleMove);
            _possibleMoves.Remove(_lastPossibleMove);//remove again since they were just used as a border

            AddFirstAndLastPossibleMoveToPossibleMovesIfMissing();
        }

        private void FillGapAt(int index)
        {
            Move gapBeginnig = _possibleMoves.ElementAt(index);
            int y;
            int gapOffset = 0;
            do
            {
                int x = (gapBeginnig.X + gapOffset + 1) % _map.SizeX;
                y = x == 0 ? gapBeginnig.Y + 1 : gapBeginnig.Y;
                if (_map.GetGridElement(x, y).GetGridType() != Data.GridType.Empty)
                {
                    if (_map.GetGridElement(x, y).GetGridType() == Data.GridType.Street)
                    {
                        _listStartingStreets.Add((x, y));
                    }
                }
                else
                {
                    _possibleMoves.Insert(index + 1, new Move(x, y));
                    break;
                }
                gapOffset++;
                //es muss abgebrochen werden soblad das andere ende von einem loch erreichtwurde
                //(das elemet existert und wir machen weiter mit index)
            } while (gapBeginnig.X + gapOffset + 1 < _possibleMoves[index + 1].X);
        }
        
        private void AddFirstAndLastPossibleMoveToPossibleMovesIfMissing()
        {
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
                (IsIllegalStreet(move)))
            {
                if (_possibleMoves.Count > 0)
                {
                    move = GetRandomMove();
                }
                else
                {
                    NoMoreValidMoves = true;
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
            int index = _possibleMoves.IndexOf(move); // index should always be 0
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
            // -> false if not a street or street invalid
            if (move.GridType != Data.GridType.Street) return false;
            return !_map.ValidateStreet(move);
        }

    
        private Move GetRandomMove()
        {
            Random random = new Random();
            Move move = _possibleMoves.ElementAt(0);

            Data.GridType toBePlaced;
            if (_map.ValidateStreet(move)
            && random.NextDouble() < _map.GetGridElement(move).getwarscheinlichkeit())
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
            return _possibleMoves.Count;
            // this is not true 
            // in 1 move  1or2 moves may be removed from _possibleMoves 
        }


        //DEBUG helper funktions
        public List<int> IsInList(Move move)
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

        public Map GetMap()
        {
            return _map;
        }
    }
}