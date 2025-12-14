using Microsoft.Maui.Hosting;
using ShoppingListArturWykurz.Services;

namespace ShoppingListArturWykurz;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).Services.AddSingleton<IDataService, JsonDataService>().AddSingleton<Views.MainPage>();

        return builder.Build();
    }
}