using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CityPlannerFrontend
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapEditor : Page
    {

        public static int X;
        public static int Y;
        public static GridTools GridTool;

        private static byte[,] _grid;
        private static Grid _gridUI;

        private readonly BitmapImage[] _textureBitmapImages;

        public MapEditor()
        {

            _textureBitmapImages = new BitmapImage[255];
            for (var i = 0; i < 255; i++)
            {
                _textureBitmapImages[i] = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + i + ".png"));
            }

            this.InitializeComponent();
            _grid = new byte[X,Y];
            _gridUI = GridGenerator(_grid);
            MapGridScrollViewer.Content = _gridUI;
        }


        public void GridTile_Click(object sender, RoutedEventArgs e)
        {
            _grid[Grid.GetRow((FrameworkElement)sender), Grid.GetColumn((FrameworkElement)sender)] = 31;
            _gridUI = GridGenerator(_grid);
            MapGridScrollViewer.Content = _gridUI;
        }


        public Grid GridGenerator(byte[,] map)
        {
            var grid = new Grid();
            var rows = map.GetLength(0);
            var cols = map.GetLength(1);


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
               
                    var img = new Image
                    {
                        Source = _textureBitmapImages[map[i, j]]
                    };
                    var tile = new Button
                    {
                        Content = img,
                        Padding = new Thickness(0),
                        BorderThickness = new Thickness(0),
                        CornerRadius = new CornerRadius(0),
                        ClickMode = ClickMode.Press
                    };
                    tile.Click += GridTile_Click;
                    grid.Children.Add(tile);
                    Grid.SetColumn(tile, j);
                    Grid.SetRow(tile, i); // set row too!
                }

            
            }

        
            return grid;
        }

    }
}
