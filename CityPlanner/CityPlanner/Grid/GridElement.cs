﻿using System.Runtime.CompilerServices;

namespace CityPlanner.Grid;

public class GridElement
{
    protected int Score ;
    protected int Level = 1;
    protected IDictionary<Data.GridType, List<double>> Dependency = new Dictionary<Data.GridType, List<double>>();

    public GridElement()
    {
        foreach (Data.GridType gridType in (Data.GridType[])Enum.GetValues(typeof(Data.GridType)))
        {
            Dependency.Add(gridType, new List<double>());
        }
    }

    public int getScore()
    {
        return Score;
    }

    protected GridElement(GridElement oldGridElement)
    {
        foreach (var dependency in oldGridElement.Dependency)
        {
            Dependency.Add( dependency.Key ,oldGridElement.Dependency[dependency.Key].ToArray().ToList());
        }
        
        Score = oldGridElement.Score;
        Level = oldGridElement.Level;
    }

    public virtual int CalculateScore()
    {
        return 0;
    }


    public void AddDependency(Data.GridType gridType, double distance)
    {
        //todo dise funkon hat gerade den 2te größten percormance impact 
        //dabei ist der get call der intensiveste
        Dependency[gridType].Add(distance);
    }

    public virtual Data.GridType GetGridType()
    {
        return Data.GridType.Empty;
    }

    public bool IsValidStreet()
    {
        Dependency[Data.GridType.Street].Sort();
        return Dependency[Data.GridType.Street].Any() && Dependency[Data.GridType.Street][0] <= 1.0;
    }

    public int GetLevel()
    {
        return Level;
    }


    public virtual GridElement Clone()
    {
        return new GridElement(this);
    }
}