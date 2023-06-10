// @author: Maximilian Koch
// Map editor page, enables the user to place map elements on the map

using CityPlanner;
using CityPlannerFrontend.UiPassing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace CityPlannerFrontend
{
    public sealed partial class MapEditor
    {
        private int _sizeX;
        private int _sizeY;
        private int _population;
        private int _importQuota;
        private int _numberAgents;
        private double _mutationChance;
        private MapTools _mapTool;
        private static byte[,] _map;
        private static Grid _grid; 
        private byte _selectedMapElement;
        private readonly Button[] _btnList;
        

      public MapEditor()
      {
         this.InitializeComponent();
         _btnList = new[] { Btn0, Btn11, Btn21, Btn32 };
      }

      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
          // Copy variables from settings page into map editors own variables
          if (e.Parameter is SettingsToMapEditor settingsToMapEditor)
          {
              _sizeX = settingsToMapEditor.GetSizeX();
              _sizeY = settingsToMapEditor.GetSizeY();
              _population = settingsToMapEditor.GetPopulation();
              _importQuota = settingsToMapEditor.GetImportQuota();
              _numberAgents = settingsToMapEditor.GetNumberAgents();
              _mutationChance = settingsToMapEditor.GetMutationChance();
              _mapTool = settingsToMapEditor.GetMapTool();
          }
          base.OnNavigatedTo(e);

          // Create and display default empty map
          _map = new byte[_sizeX, _sizeY];
          _grid = FillMap(_map);
          MapGridScrollViewer.Content = _grid;
      }

      // Create from 2d byte array a corresponding ui control grid 
      private Grid FillMap(byte[,] map)
      {
          var rows = map.GetLength(0);
          var cols = map.GetLength(1);
          var grid = MapTools.PrepareEmptyMap(rows, cols);
          
          // 3. Add each item and set row and column
          for (var i = 0; i < rows; i++)
          {
              for (var j = 0; j < cols; j++)
              {
                  grid = ChangeSingleMapElement(map[i, j], grid, i, j);
              }
          }
          return grid;
      }
        
      // Change a single map element in the ui control grid
      private Grid ChangeSingleMapElement(byte mapElementId, Grid grid, int row, int col)
      {
          // Prepare needed texture image ui control
          var img = new Image
         {
            Source = _mapTool.GetTextureBitmapImages()[mapElementId]
         };
         // Create button ui control with texture image as content
         var tile = new Button
         {
            Content = img,
            Padding = new Thickness(0),
            BorderThickness = new Thickness(0),
            CornerRadius = new CornerRadius(0),
            ClickMode = ClickMode.Press
         };
         tile.Click += BtnMapElement;

         // Add created button ui control to grid
         grid.Children.Add(tile);
         Grid.SetRow(tile, row);
         Grid.SetColumn(tile, col);

         return grid;
      }

      // Change a single map element in the map to the selected map element from the toolbar if user clicks on it
      public void BtnMapElement(object sender, RoutedEventArgs e)
      {
          var row = Grid.GetRow((FrameworkElement)sender);
          var col = Grid.GetColumn((FrameworkElement)sender);
          _map[row, col] = _selectedMapElement;
          _grid.Children.Remove((FrameworkElement)sender);
          MapGridScrollViewer.Content = ChangeSingleMapElement(_selectedMapElement, _grid, row, col);
      }

      // Change the selected map element to the one the user clicked on in the toolbar
      private void BtnMapElementToolBar(object sender, RoutedEventArgs e)
      {
         var btn = byte.Parse(((Button)sender).Tag.ToString() ?? "0");
         _selectedMapElement = btn;

         foreach (var button in _btnList)
         {
            button.BorderThickness = new Thickness(0);
            button.BorderBrush = new SolidColorBrush(Colors.Transparent);
         }

         // Switch over all button tags to set the border of the selected button
         switch (btn)
         {
            case 0:
               Btn0.BorderThickness = new Thickness(3);
               Btn0.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 11:
               Btn11.BorderThickness = new Thickness(3);
               Btn11.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 21:
               Btn21.BorderThickness = new Thickness(3);
               Btn21.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
            case 32:
               Btn32.BorderThickness = new Thickness(3);
               Btn32.BorderBrush = new SolidColorBrush(Colors.Red);
               break;
         }
      }

      // Create api to start simulation and navigate to map view
      private void BtnMapView(object sender, RoutedEventArgs e)
      { 
         Api api = new (_population, _map, _importQuota, _numberAgents, _mutationChance*100.0);
         ToMapView toMapView = new (_sizeX, _sizeY, _population, _importQuota, _numberAgents, _mutationChance, _mapTool, api);
         Frame.Navigate(typeof(MapView), toMapView);
      }

      // Navigate back to settings page
      private void BtnSettings(object sender, RoutedEventArgs e)
      {
         ToSettings toSettings = new (_sizeX, _sizeY, _population, _importQuota, _numberAgents, _mutationChance);
         Frame.Navigate(typeof(Settings), toSettings);
      }
    }
}
