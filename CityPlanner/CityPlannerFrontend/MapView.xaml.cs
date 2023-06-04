// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CityPlannerFrontend
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {

        private bool pause = false;
        
        public static API Interface { get; set; }
        


        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public MapView()
        {
            this.InitializeComponent();

            Task task = new(() => { _ = BackendLoopAsync(); });
            task.Start();

        }

        private Task BackendLoopAsync()
        {
            Debug.WriteLine("entered BackendLoop");
            if (Interface == null) return Task.CompletedTask;
            while (!pause)
            {
                Debug.WriteLine("Next Generation");
                Interface.nextGeneration();
                

                _dispatcherQueue.TryEnqueue(() =>
                {
                    Generation.Text = Interface.getGeneration().ToString();
                });

                if (!Interface.existsNewMap()) continue;
                Debug.WriteLine("New Map");

                _dispatcherQueue.TryEnqueue(() =>
                {
                    // Update UI elements with the updated variable values
                    FillGrid(Interface.getMapToFrontend());    
                    satisfaction.Text = Interface.getSatisfaction().ToString();
                    Gridcount.Text = Interface.getPlacedBuildings().ToString();
                    Blevel.Text = Interface.getAverageBuildLevel().ToString();
                    Population.Text = Interface.getPopulation().ToString();
                });
            }

            return Task.CompletedTask;
        }

 

        private void Pause_onclick(object sender, RoutedEventArgs e) {
            if (pause)
            {
                Task.Run(() => { BackendLoopAsync(); });
                MapGridScrollViewer.Opacity = 1;
                PauseButton.Content = "Pause";
            }
            else
            {
                MapGridScrollViewer.Opacity = 0.6;
                PauseButton.Content = "Fortsetzen";
            }
            pause = !pause;
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            pause = true;
            Frame.Navigate(typeof(Settings));
        }

        private void FillGrid(byte[,] map)
        {
            var mapGrid = GridGenerator(map);
            MapGridScrollViewer.Content = mapGrid;;
        }


        private static Grid GridGenerator(byte[,] map)
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
                    var tile = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + map[i, j] + ".png"))
                    };
                    grid.Children.Add(tile);
                    Grid.SetColumn(tile, j);
                    Grid.SetRow(tile, i); // set row too!
                }
            }
            return grid;
        }
       
        
    }
}