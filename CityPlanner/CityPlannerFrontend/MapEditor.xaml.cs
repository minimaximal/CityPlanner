using Microsoft.UI.Xaml.Controls;

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


        public MapEditor()
        {
            this.InitializeComponent();
        }
    }
}
