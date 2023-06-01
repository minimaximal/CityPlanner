// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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

        public MapView()
        {
            this.InitializeComponent();
            Grid mapGrid = GridGenerator(Map.GetLength(0), Map.GetLength(1), Map);
            mapGrid.SetValue(Grid.ColumnProperty, 1);
            LayoutRoot.Children.Add(mapGrid);
            Task task = new Task(() => { BackendLoop(); });
            task.Start();
            //Task.Run(() => { BackendLoop(); });
            
        }

        private void BackendLoop()
        {
            Debug.WriteLine("entered BackendLoop");
            if (Interface != null)
            {
                //while (!pause)
                //{
                    Debug.WriteLine("Next Generation");
                    Interface.nextGeneration();
                    if (Interface.existsNewMap()){
                        Debug.WriteLine("New Map");
                        Interface.getMapToFrontend();
                        Satisfaction = Interface.getSatisfaction();
                        Rastercount = Interface.getPlacedBuildings();
                        Buildinglevel = Interface.getAverageBuildLevel();
                        Population = Interface.getPopulation();
                        Debug.WriteLine(Interface.getSatisfaction());

                    Rasternazhl.Text = Interface.getSatisfaction().ToString();
                    /*.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {

                        setVariables();
                    });*/
                    Task.Run(() => { BackendLoop(); });
                    return;
                }
                    Thread.Sleep(1000);
                    
               // }
                
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
                Task.Run(() => { BackendLoop(); });
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
