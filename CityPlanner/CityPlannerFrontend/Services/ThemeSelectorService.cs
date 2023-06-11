using Microsoft.UI.Xaml;
using System.Threading.Tasks;


namespace CityPlannerFrontend.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    public ElementTheme Theme { get; set; } = ElementTheme.Default;


    public async Task InitializeAsync()
    {
        const ElementTheme elementTheme = (ElementTheme)1;
        Theme = elementTheme;
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();

    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;
        }

        await Task.CompletedTask;
    }


}
