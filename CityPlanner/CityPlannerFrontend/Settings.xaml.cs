// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CityPlannerFrontend
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public int Population=10000;
        public int ImportQuota = 10; // 0 bis 100 (percent)
        public int X=50;
        public int Y=30;
        private readonly MapTools _gridTool = new();
        
        public Settings()
        {
            this.InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click_MapView(object sender, RoutedEventArgs e)
        {
            if (!inputValidation()) return;
            var map = new byte[X,Y];
            var @interface = new API(Population, map, ImportQuota);
            MapView.Interface = @interface;
            MapView.MapTool = _gridTool;
            Frame.Navigate(typeof(MapView));
        }


        private void Button_Click_MapEditor(object sender, RoutedEventArgs e)
        {
            if (!inputValidation()) return;
            MapEditor.Population = Population;
            MapEditor.ImportQuota = ImportQuota;
            MapEditor.X = X;
            MapEditor.Y = Y;
            MapEditor.MapTool = _gridTool;
            Frame.Navigate(typeof(MapEditor));
        }



            private bool inputValidation()
            {
                return X != 0 && Y != 0;
                // TODO Warning Popup that Map needs at least size ...
            }
    }
}
