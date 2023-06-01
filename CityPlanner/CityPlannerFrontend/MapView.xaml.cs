// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
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
    public sealed partial class MapView : Page
    {
        public int Satisfaction = 0;
        public int Buildinglevel = 0;
        public int Rastercount = 0;
        public int Population = 0;
        private bool pause = false;
        
        public static API Interface { get; set; }

        public byte[,] Map = new byte[3, 3] { { 31, 31, 31 }, { 122, 121, 121 }, { 133, 133, 133 } };

        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public MapView()
        {
            this.InitializeComponent();
            Grid mapGrid = GridGenerator(Map.GetLength(0), Map.GetLength(1), Map);
            mapGrid.SetValue(Grid.ColumnProperty, 1);
            LayoutRoot.Children.Add(mapGrid);
            
            Task task = new Task(() => { BackendLoopAsync(); });
            task.Start();
            //Task.Run(() => { BackendLoop(); });
            
        }

        private async Task BackendLoopAsync()
        {
            Debug.WriteLine("entered BackendLoop");
            if (Interface != null)
            {
                while (!pause)
                {
                    Debug.WriteLine("Next Generation");
                    Interface.nextGeneration();
                    Debug.WriteLine(Interface.existsNewMap());
                    if (Interface.existsNewMap()){
                        Debug.WriteLine("New Map");
                        
                        
                        Rastercount = Interface.getPlacedBuildings();
                        Population = Interface.getPopulation();
                        //Buildinglevel = Interface.getAverageBuildLevel();
                        _dispatcherQueue.TryEnqueue(() =>
                    {
                        // Update UI elements with the updated variable values
                        var Map2 = Interface.getMapToFrontend();
                        Grid mapGrid = GridGenerator(Map2.GetLength(0), Map2.GetLength(1), Map2);
                        mapGrid.SetValue(Grid.ColumnProperty, 1);
                        LayoutRoot.Children.Add(mapGrid);
                        satisfaction.Text = Interface.getSatisfaction().ToString();
                        Rasternazhl.Text = Interface.getPlacedBuildings().ToString();
                        // Update other UI elements here
                    });
                }
                    Thread.Sleep(1000);
                    
                }
                
            }
        }

        private void setVariables()
        {
            Interface.getMapToFrontend();
            Satisfaction = Interface.getSatisfaction();
            Rastercount = Interface.getPlacedBuildings();
            Buildinglevel = Interface.getAverageBuildLevel();
            Population = Interface.getPopulation();

        }

        private void Pause_onclick(object sender, RoutedEventArgs e) {
            if (pause)
            {
                Task.Run(() => { BackendLoopAsync(); });
            }
            pause = !pause;
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
