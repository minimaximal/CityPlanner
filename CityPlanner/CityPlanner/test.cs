using CityPlanner.Grid;


namespace CityPlanner;

using CityPlanner;

public class test
{
   public static bool testCopyofGrid_Deep()
    {
        GridElement n1 = new GridElement();
        Housing haus = new Housing(n1);
        haus.AddDependency(Data.GridType.Street, 1);
        GridElement n2 = n1.Clone();
        Industry industry = new Industry(n2);
        GridElement industry2 = industry.Clone();


        // todo ich weiß nicht ob dies existerit aber eigentlich würde man hier ein assert() nutzen 
        // denk darann ggf die bedingungen zu ivertierten 
        
        // todo für diese test benötigen wir acses zu Dependency
       // if (haus.Dependency[Data.GridType.Street].Count == 1) return false;
       // if (industry.Dependency[Data.GridType.Street].Count != 0) return false;
        if (industry.GetGridType() != industry2.GetGridType()) return false;
        if (industry == industry2) return false;  // clonen gescheitet es ist eine reference

        return true;
    }


   public static bool testMoveSort()
    {
        Data.SizeX = 5;
        List<Move> istListe = new List<Move>();

        istListe.Add(new Move(3, 1));
        istListe.Add(new Move(1, 3));
        istListe.Add(new Move(1, 1));
        istListe.Add(new Move(1, 4));
        istListe.Add(new Move(1, 2));
        istListe.Add(new Move(3, 1));
        istListe.Add(new Move(3, 2));

        istListe.Insert(4, new Move(5, 5));

        List<Move> sollListe = new List<Move>();

        sollListe.Add(new Move(1, 1));
        sollListe.Add(new Move(3, 1));
        sollListe.Add(new Move(3, 1));
        sollListe.Add(new Move(1, 2));
        sollListe.Add(new Move(3, 2));
        sollListe.Add(new Move(1, 3));
        sollListe.Add(new Move(1, 4));

        sollListe.Insert(7, new Move(5, 5));
        
        
        istListe.Sort();
        
        foreach (var move in istListe)
        {
            if (sollListe.ElementAt(istListe.IndexOf(move)).X == move.X
                && sollListe.ElementAt(istListe.IndexOf(move)).Y == move.Y)
            {
            }
            else
            {
                return false; // test faield 
            }
        }

        return true;
    }
}