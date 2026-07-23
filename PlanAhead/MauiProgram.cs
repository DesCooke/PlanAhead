using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PlanAhead.Extensions;
using PlanAhead.Core.Extensions;
using PlanAhead.Infrastructure.Extensions;

namespace PlanAhead
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "planahead.db");

            builder.Services.AddPlanAhead();
            builder.Services.AddPlanAheadCore();
            builder.Services.AddPlanAheadInfrastructure(dbPath);


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
