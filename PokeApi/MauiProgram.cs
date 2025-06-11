using Microsoft.Extensions.Logging;

namespace PokeApi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("pokemonsolid.ttf", "PokemonSolid");
                    fonts.AddFont("PressStart2P-Regular.ttf", "Ps2P");
                    fonts.AddFont("Kanit-Light.ttf", "KanitL");
                    fonts.AddFont("Kanit-Regular.ttf", "KanitR");
                    fonts.AddFont("Fredoka_Condensed-Regular", "Fredoka");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
