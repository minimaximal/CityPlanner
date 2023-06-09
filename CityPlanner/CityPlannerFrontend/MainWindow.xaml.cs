// @author: Leo Schnüll
// Main and only window of the application. It is responsible for setting the title of the window.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;


namespace CityPlannerFrontend
{
    public sealed partial class MainWindow
   {
      public MainWindow()
      {
         this.InitializeComponent();

         var mAppWindow = GetAppWindowForCurrentWindow();
         mAppWindow.Title = "City Planner";
      }
      
      private AppWindow GetAppWindowForCurrentWindow()
      {
         var hWnd = WindowNative.GetWindowHandle(this);
         var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
         return AppWindow.GetFromWindowId(wndId);
      }
   }
}