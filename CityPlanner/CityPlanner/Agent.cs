namespace CityPlanner
{
    public class Agent
    {
        private Map _map;
        private List<Move> _moves = new List<Move>();
        private int _moveCounter ;
        private List<Move> _emptyMoves = new List<Move>();
        public bool NoMoreValidStreet;
        
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
            NoMoreValidStreet = false;
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

            if (_moves.Count> map.SizeX*map.SizeY)
            {
                Console.Write("wierd but legal");
            }
        }

        public void MakeOneMove()
        {
            Random random = new Random();
            Move move = null;
            bool flag = false;
            if (_moves.Count > _moveCounter)
            {
                move = new Move( _moves.ElementAt(_moveCounter));
                flag = true;
                foreach (Move findMove in _emptyMoves) //will be iterated Rand(0 , sizeX*SizeY) times O(log(n^2))
                {
                    if (findMove.X == move.X && findMove.Y == move.Y)
                    {
                        _emptyMoves.Remove(findMove);
                        break;
                    }
                }
            }

            if (move == null || random.NextDouble() < 0.015 ||
                (!IsLegalMove(move)) ||
                (!IsLegalStreet(move)))
            {
                if (flag && IsLegalMove(move))
                {
                    _emptyMoves.Add(move);
                }
                move = GetRandomMove();
              
            }
            
            _moves.Insert(_moveCounter,move); 
            _map.AddMove(move);
            _moveCounter++;
        }

        private bool IsLegalMove(Move move)
        {
            return _map.GetGridElement(move).GetGridType() == Data.GridType.Empty;
        }

        private bool IsLegalStreet(Move move)
        {
            //if not a street -> false
            //if this move is a street ->  is legal street?
            return (move.GridType == Data.GridType.Street && _map.ValidateStreet(move));
        }


        private Move GetRandomMove()
        {
            // copy pasted from https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
            Array values = Enum.GetValues(typeof(Data.GridType));
            Random random = new Random();
            Data.GridType toBePlaced;
            do
            {
                toBePlaced = (Data.GridType)(values.GetValue(random.Next(values.Length - 1)));
            } while (NoMoreValidStreet && toBePlaced == Data.GridType.Street);

            //end copy

            Move move;
            if (toBePlaced == Data.GridType.Street)
            {
                move = GetRandomStreet();
            }
            else
            {
                var rand = random.Next(0, _emptyMoves.Count);
                move = _emptyMoves[rand];
            }


            move.GridType = toBePlaced;
            _emptyMoves.Remove(move); //in emptyMove are only new moves 
            return move;
        }

        Move GetRandomStreet()
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
                NoMoreValidStreet = true;
                return GetRandomMove(); //todo maybe no recursion ??
            }

            int rand = random.Next(0, limitedMoves.Count);
            return limitedMoves[rand];
        }
        
        public void Display()
        {
            _map.NewDisplay();
        }

        public int GetMaxRemainingMoves()
        {
            return _emptyMoves.Count();
        }
    }
}