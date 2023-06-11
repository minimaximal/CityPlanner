// @author: Maximilian Koch
// class for holding map element textures and preparing empty maps according to the given size

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace CityPlannerFrontend;

public class MapTools
{

    private readonly BitmapImage[] _textureBitmapImages;

    public MapTools()
    {
        // Load bitmap images for map elements from assets folder and store them in an array
        // Since the map elements are represented by byte values, the array index is the byte value
        // All not existing files will be ignored and the corresponding array element will be null
        // Since this is a static array, it will be loaded only once and the performance effect for not found files is negligible
        _textureBitmapImages = new BitmapImage[255];
        for (var i = 0; i < 255; i++)
        {
            _textureBitmapImages[i] = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + i + ".png"));
        }
    }

    // Create with the given size a corresponding ui control grid
    public static Grid PrepareEmptyMap(int rows, int cols)
    {
        var grid = new Grid();

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

        return grid;
    }

    public BitmapImage[] GetTextureBitmapImages()
    {
        return _textureBitmapImages;
    }
}