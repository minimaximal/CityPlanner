// @author: Leo Schnüll

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CityPlannerFrontend
{
   public sealed partial class MainPage : Page
   {
      public MainPage()
      {
         this.InitializeComponent();
      }

      private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
      {
         Frame.Navigate(typeof(Settings));
      }
   }
}
