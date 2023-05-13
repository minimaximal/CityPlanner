using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityPlanner.Grid;

namespace CityPlanner
{
    // jeden move werden felder welche betroffen werden können informiert 
    // agent pickt ein lehres feld 
    // agent pickt ein gebeude welches plaziert werden soll
    // von dem plazierten gebäude werden alle felder in range R aktualiesiert 
    // bekommen die info gebeude bei X Y                    (das sollte vllt eine referenz sein)
    // alle 20 moves wird der score berechnet und geschaut wie viele einwohner die stadt hat
    //iteratiev durch das array gehen und caculateScore() aufrufen 
    // jedes GridElement geht duch die liste der gebäude durch
    // jenach relevanz wird der core berechenet
    // alle Zellen scores addieren 


    // ggf abruch


    public class Agent
    {
        GridElement[,] Map = new GridElement[100, 100];
        private List<Move> _moves = new List<Move>();
        private List<Move> _emptyMoves = new List<Move>();


        public Agent(int size_x, int size_y) : this(size_x, size_y, new GridElement[100, 100])
        {
        }

        public Agent(int size_x, int size_y, GridElement[,] map)
        {
            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    if (map[x, y].GetGridType() == Data.GridType.Empty)
                    {
                        _emptyMoves.Add(new Move(x, y));
                    }
                }
            }
        }

        Move getRandomMove()
        {
            // copy pasted from https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
            Array values = Enum.GetValues(typeof(Data.GridType));
            Random random = new Random();
            Data.GridType toBePlaced = (Data.GridType)values.GetValue(random.Next(values.Length));
            //end copy

            int rand = 0;
            if (toBePlaced == Data.GridType.Street)
            {
                rand = _emptyMoves.IndexOf(getRandomStreet());
            }
            else
            {
                rand = random.Next(0, _emptyMoves.Count);
            }
            
            Move move = _emptyMoves[rand];
            move.GridType = toBePlaced;
            _emptyMoves.RemoveAt(rand);
            return move;
        }

        Move getRandomStreet()
        {
            Random random = new Random();
            
            List<Move> limitedMoves = new List<Move>();
            foreach (var possibleStreet in _emptyMoves)
            {
                if (Map[possibleStreet.X,possibleStreet.Y].IsValidStreet())
                {
                    limitedMoves.Add(possibleStreet);
                }
            }
            int rand = random.Next(0, _emptyMoves.Count);
            return limitedMoves[rand];
        }
        

        void addMoveToMap(Move move)
        {
            // update GridElement to sup class
            Map[move.X, move.Y] = newGridElement(move.GridType, Map[move.X, move.Y]);
            //inform in range of this new exisitance
            
        }

        GridElement newGridElement(Data.GridType gridType, GridElement old)
        {
            switch (gridType)
            {
                case Data.GridType.Housing:
                    return new Housing(old);
                case Data.GridType.Industry:
                    return new Industry(old);
                case Data.GridType.Street:
                    return new Street(old);
                case Data.GridType.Commercial:
                    return new Commercial(old);
                case Data.GridType.Empty:
                    return old;
            }
            throw new Exception("In dem Switch case sollten alle GridTypes abgedeckt sein");
        }
        

    }
}