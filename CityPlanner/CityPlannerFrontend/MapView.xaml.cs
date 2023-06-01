// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CityPlannerFrontend
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {
        public int Satisfaction = 0;
        public int Buildinglevel = 0;
        public int Rastercount = 0;

        public byte[,] Map = new byte[,] { { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 }, { 31, 31, 31 }, { 31, 31, 31 }, { 31, 31, 31 } };

        public MapView()
        {
            this.InitializeComponent();
            var mapGrid = GridGenerator(Map.GetLength(0), Map.GetLength(1), Map);
            MapGridScrollViewer.Content = mapGrid;

        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private static Grid GridGenerator(int rows, int cols, byte[,] map)
        {
            var grid = new Grid();

            // 1. prepare RowDefinitions
            for (var i = 0; i < rows; i++)
            {
                var row = new RowDefinition
                {
                    Height = new GridLength(0, GridUnitType.Auto)
                };
                grid.RowDefinitions.Add(row);
            }

            // 2. prepare ColumnDefinitions
            for (var j = 0; j < cols; j++)
            {
                var column = new ColumnDefinition
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                };
                grid.ColumnDefinitions.Add(column);
            }

            // 3. add each item and set row and column
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var tile = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + map[i, j] + ".png"))
                    };
                    grid.Children.Add(tile);
                    Grid.SetColumn(tile, j);
                    Grid.SetRow(tile, i); // Set row too!
                }
            }
            return grid;
        }
    }
}
