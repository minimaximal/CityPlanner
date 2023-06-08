// @author: Maximilian Koch

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace CityPlannerFrontend;

public class MapTools
{

   private readonly BitmapImage[] _textureBitmapImages;

   public MapTools()
   {
      _textureBitmapImages = new BitmapImage[255];
      for (var i = 0; i < 255; i++)
      {
         _textureBitmapImages[i] = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + i + ".png"));
      }
   }


   public Grid MapGenerator(byte[,] map)
   {
      var grid = new Grid();
      var rows = map.GetLength(0);
      var cols = map.GetLength(1);


      // 1. Prepare RowDefinitions
      for (var i = 0; i < rows; i++)
      {
         var row = new RowDefinition
         {
            Height = new GridLength(0, GridUnitType.Auto)
         };
         grid.RowDefinitions.Add(row);
      }

      // 2. Prepare ColumnDefinitions
      for (var j = 0; j < cols; j++)
      {
         var column = new ColumnDefinition
         {
            Width = new GridLength(0, GridUnitType.Auto)
         };
         grid.ColumnDefinitions.Add(column);
      }

      // 3. Add each item and set row and column
      for (var i = 0; i < rows; i++)
      {
         for (var j = 0; j < cols; j++)
         {
            var tile = new Image
            {
               Source = _textureBitmapImages[map[i, j]]
            };
            grid.Children.Add(tile);
            Grid.SetColumn(tile, j);
            Grid.SetRow(tile, i);
         }
      }
      return grid;
   }
}