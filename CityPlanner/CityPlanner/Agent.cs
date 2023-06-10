// @author: Sander Stella, Paul Antoni, Kevin Kern

namespace CityPlanner
{
    public class Agent
    {
        private Map _map = null!;
        private readonly List<Move> _parentMoves = new();
        private readonly List<Move> _moves = new();
        private List<Move> _possibleMoves = new();
        private static Move _firstPossibleMove = null!;
        private static Move _lastPossibleMove = null!;

        private int _streetCounter;
        public bool NoMoreValidMoves;
        public int Score;

        // Basic Constructor for first gen of Agents
        public Agent(Map map)
        {
            BasicSetup(map);
        }

        /*advanced Constructor that contains previous Agents for Mixture as well as
        split Point that tells how much to take from which parent*/
        public Agent(Map map, Agent parent1, Agent parent2, double split)
        {
            _parentMoves = SelectMovesFromParents(parent1, parent2, split);
            BasicSetup(map);
        }

        private void BasicSetup(Map map)
        {
            _map = map;
            _firstPossibleMove = new Move(0, 0);
            _lastPossibleMove = new Move(_map.SizeX - 1, _map.SizeY - 1);
            NoMoreValidMoves = false;
            _possibleMoves.AddRange(_parentMoves);
            _possibleMoves = _possibleMoves.OrderBy(move => move.IndexNumber()).ToList();

            FillGapsInMovesList();
            _possibleMoves = _possibleMoves.OrderBy(move => move.DistanceToCenter()).ToList();
        }

        // Splits moves of parents at splitPoint and generates new move list
        private static List<Move> SelectMovesFromParents(Agent parent1, Agent parent2, double split)
        {
            List<Move> result = new();
            if (split >= 1)
            {
                split = 1;
            }

            var shorterParentCount = parent1._moves.Count;

            if (parent2._moves.Count < parent1._moves.Count)
            {
                shorterParentCount = parent2._moves.Count;
            }

            for (var i = 0; i < shorterParentCount * split; i++)
            {
                result.Add(new Move(parent1._moves.ElementAt(i)));
            }

            for (var i = result.Count; i < parent2._moves.Count; i++)
            {
                result.Add(new Move(parent2._moves.ElementAt(i)));
            }


            if (shorterParentCount != parent2._moves.Count) return result;
            {
                for (var i = result.Count; i < parent1._moves.Count; i++)
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

            for (var i = 0; i < _possibleMoves.Count - 1; i++)
            {
                if (_possibleMoves[i].IndexNumber() - _possibleMoves[i + 1].IndexNumber() < -1)
                {
                    FillGapAt(i);
                }
            }

            _possibleMoves.Remove(_firstPossibleMove);
            _possibleMoves.Remove(_lastPossibleMove); // Remove moves again since they were just used as a border

            AddFirstAndLastPossibleMoveToPossibleMovesIfMissing();
        }

        // Gets called by FillGapsInMovesList()
        private void FillGapAt(int index)
        {
            var gapBeginning = _possibleMoves.ElementAt(index);
            var gapOffset = 0;
            do
            {
                var tmp = (gapBeginning.X + gapOffset + 1);
                var x = tmp % _map.SizeX;
                var y = gapBeginning.Y + tmp / _map.SizeX;
                if (_map.GetGridElement(x, y)!.GetGridType() == Data.GridType.Empty)
                {
                    _possibleMoves.Insert(index + 1, new Move(x, y));
                    break;
                }

                gapOffset++;
            } while (gapBeginning.IndexNumber() + gapOffset + 1 < _possibleMoves[index + 1].IndexNumber());
        }

        // Gets called by FillGapsInMovesList() in order to add first and last moves again if they are necessary
        private void AddFirstAndLastPossibleMoveToPossibleMovesIfMissing()
        {
            if (_possibleMoves[0].IndexNumber() != _firstPossibleMove.IndexNumber())
            {
                if (_map.GetGridElement(_firstPossibleMove)!.GetGridType() == Data.GridType.Empty)
                {
                    _possibleMoves.Insert(0, new Move(_firstPossibleMove));
                }
            }

            if (_possibleMoves[^1].IndexNumber() == _lastPossibleMove.IndexNumber()) return;
            if (_map.GetGridElement(_lastPossibleMove)!.GetGridType() == Data.GridType.Empty)
            {
                _possibleMoves.Add(new Move(_lastPossibleMove));
            }
        }

        public void CalculateScore()
        {
            Score = _map.CalculateScore();
        }

        public void MakeNMoves(int n)
        {
            for (var move = 0; move < n; move++)
            {
                MakeOneMove();
            }
        }

        // Gets called by MakeNMoves(int)
        public void MakeOneMove()
        {
            var random = new Random();
            Move? move = null;
            if (_parentMoves.Count > 0)
            {
                do
                {
                    move = _parentMoves[0];
                    _parentMoves.RemoveAt(0);
                    // If parent move is blocked continue on with the next
                } while (_parentMoves.Count > 0 && !_possibleMoves.Contains(move));
            }

            if (move == null || random.NextDouble() < Data.MutationChance ||
                (IsIllegalStreet(move)))
            {
                if (_possibleMoves.Count > 0)
                {
                    move = GetRandomMove();
                }
                else
                {
                    NoMoreValidMoves = true;
                    // Stop
                    return;
                }
            }

            RemoveFromPossibleMoves(move);
            _moves.Add(move);
            _map.AddMove(move);
        }

        // Returns false if not a street or street invalid
        private bool IsIllegalStreet(Move move)
        {
            if (move.GridType != Data.GridType.Street) return false;
            return !_map.ValidateStreet(move);
        }

        public static bool IsOnGrid(Move move)
        {
            var xOff = Data.InitialStreets[0].Item1 % 7;
            return (move.X % 7 == xOff || move.Y % 7 == xOff);
        }

        public bool ShouldUtiliseGrid()
        {
         
            var random = new Random();
            return random.NextDouble() < (-0.025 * _streetCounter + 2);
        }


        public bool ShouldTheAgentBuildAStreet(Move move)
        {
            var random = new Random();
            if (IsOnGrid(move) && ShouldUtiliseGrid())
            {
                return true;
            }
            return random.NextDouble() < _map.GetGridElement(move)!.GetProbability();
        }

        private Move GetRandomMove()
        {
            var random = new Random();

            var pick = 0;
            if (_moves.Count>10 && _possibleMoves.Count > 10)
            {
                pick = random.Next() % 10;
            }

            var move = _possibleMoves.ElementAt(pick);
            Data.GridType toBePlaced;
            if (_map.ValidateStreet(move)
                && ShouldTheAgentBuildAStreet(move))
            {
                toBePlaced = Data.GridType.Street;
                _streetCounter++;
            }
            else
            {
                var rand = random.NextDouble();
                toBePlaced = rand switch
                {
                    // 50% Chance
                    < 0.5 => Data.GridType.Housing,
                    // 28% Chance
                    < 0.78 => Data.GridType.Commercial,
                    // 15% Chance
                    < 0.93 => Data.GridType.Industry,
                    // 5% Chance
                    < 0.98 => Data.GridType.Subway,
                    _ => Data.GridType.Sight
                };
            }

            if (toBePlaced == Data.GridType.Industry && Data.OptimalIndustryAmount == 0)
                toBePlaced = Data.GridType.Housing;
                
            move.GridType = toBePlaced;
            return move;
        }

        // Gets called by MakeOneMove() in order to remove an already used move
        private void RemoveFromPossibleMoves(Move move)
        {
            var index = _possibleMoves.IndexOf(move); // Index should always be 0
            // Removes the move from possibleMoves list and checks sorted neighbors for dupes 
            if (index > 0)
            {
                var inFrontOf = _possibleMoves[index - 1];
                if (inFrontOf.X == move.X &&
                    inFrontOf.Y == move.Y)
                {
                    _possibleMoves.Remove(inFrontOf);
                }
            }

            if (index + 1 < _possibleMoves.Count)
            {
                var justBehind = _possibleMoves[index + 1];
                if (justBehind.X == move.X &&
                    justBehind.Y == move.Y)
                {
                    _possibleMoves.Remove(justBehind);
                }
            }

            _possibleMoves.Remove(move);
        }

        public Map GetMap()
        {
            return _map;
        }


        #region DEBUG

        public List<int> IsInList(Move move)
        {
            return (from m in _possibleMoves where m.X == move.X && m.Y == move.Y select _possibleMoves.IndexOf(m)).ToList();
        }

        #endregion
    }
}