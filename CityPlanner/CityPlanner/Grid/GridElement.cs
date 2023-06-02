﻿using System.Runtime.CompilerServices;

namespace CityPlanner.Grid;

public class GridElement
{
    protected int Score;
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
            Dependency.Add(dependency.Key, oldGridElement.Dependency[dependency.Key].ToArray().ToList());
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

    private double fn(double x)
    {
        return 5 / (x - 24) + 1;
    }
    private double fn2(double x)
    {
        return  0.8-0.2*(x );
    }
   

    public double getwarscheinlichkeit()
    {
        //Retuns valu beeween 0 and 1 

        Dependency[Data.GridType.Street].Sort();
        int counter = 0;
        while (Dependency[Data.GridType.Street].Count>counter && Dependency[Data.GridType.Street][counter] <= 1)
        {
            counter++;
        }
        

        return fn2(counter);
    }
}