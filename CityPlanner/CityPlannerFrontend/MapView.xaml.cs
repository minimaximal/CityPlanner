// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using System.Globalization;
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
        public static API Interface { get; set; }
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private bool _pause = false;
        
        private readonly BitmapImage[] _textureBitmapImages;

        private string _gridCount;
        private string _satisfaction;
        private string _avarageBuildingLevel;
        private string _population;
        private string _generationCount;
        private string _lastNewMap;
        

        public MapView()
        {
            this.InitializeComponent();

            _textureBitmapImages = new BitmapImage[255];
            for (var i = 0; i < 255; i++)
            {
                _textureBitmapImages[i] = new BitmapImage(new Uri("ms-appx:///Assets//Grid//" + i + ".png"));
            }

            Task task = new(() => { _ = BackendLoopAsync(); });
            task.Start();
        }

        private Task BackendLoopAsync()
        {
            Debug.WriteLine("entered BackendLoop");
            if (Interface == null) return Task.CompletedTask;
            while (!_pause)
            {
                Debug.WriteLine("Next Generation");
                Interface.nextGeneration();
                Debug.WriteLine(Interface.existsNewMap());


                // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
                _generationCount = Interface.getGeneration().ToString();

                _dispatcherQueue.TryEnqueue(() =>
                {
                    // update UI elements with the updated variable values
                    Generation.Text = _generationCount;
                });

                if (!Interface.existsNewMap()) continue;
                Debug.WriteLine("New Map");

                // saved in variable before because of multithreading, makes dispatchers execution time shorter and less likely to fail / show wrong or old values
                _gridCount = Interface.getPlacedBuildings().ToString();
                _satisfaction = Interface.getSatisfaction().ToString();
                _avarageBuildingLevel = Interface.getAverageBuildLevel().ToString(CultureInfo.InvariantCulture);
                _population = Interface.getPopulation().ToString();
                _lastNewMap = _generationCount;

                _dispatcherQueue.TryEnqueue(() =>
                {
                    // update UI elements with the updated variable values
                    MapGridScrollViewer.Content = GridGenerator(Interface.getMapToFrontend()); // for MapGrid it's not possible to prepare the updated grid in advance because it's a nested object
                    GridCount.Text = _gridCount;
                    Satisfaction.Text = _satisfaction;
                    AvarageBuildingLevel.Text = _avarageBuildingLevel;
                    Population.Text = _population;
                    LastNewMap.Text = _lastNewMap;
                });
            }

            return Task.CompletedTask;
        }

 

        private void Pause_onclick(object sender, RoutedEventArgs e) {
            if (_pause)
            {
                Task.Run(() => { BackendLoopAsync(); });
            }
            _pause = !_pause;
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

    
        private Grid GridGenerator(byte[,] map)
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
                        Source = _textureBitmapImages[map[i, j]]   
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