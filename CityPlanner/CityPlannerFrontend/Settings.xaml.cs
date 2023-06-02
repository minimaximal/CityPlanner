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
        public int IEinwohnerzahl=10000;
        public int X=50;
        public int Y=30;
        public int Importquota = 10;//0 bis 100 (prozent)
        public Settings()
        {
            this.InitializeComponent();
            
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (X != 0 && Y != 0)
            {
                API Interface = new API(IEinwohnerzahl, X, Y, Importquota);
                MapView.Interface = Interface;
                Frame.Navigate(typeof(MapView));
            }
            else { 
            // TODO Warning Popup that Map needs at least size ...
            }
        }
    }
}
