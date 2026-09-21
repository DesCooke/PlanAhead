using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PlanAhead.Core.Extensions;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.MethodLogging;
using PlanAhead.Extensions;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Extensions;
using PlanAhead.Infrastructure.Logging;
using Supabase;
using PlanAhead.Interfaces;
using System.Diagnostics;


#if ANDROID
using Android.Text;
using Microsoft.Maui.Handlers;
#endif

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
                    fonts.AddFont("cour.ttf", "CourierNew");
                });

            string dbPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "planahead.db");

            builder.Services.AddPlanAhead();
            builder.Services.AddPlanAheadCore();
            builder.Services.AddPlanAheadInfrastructure(dbPath);

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = false
            };

            var client = new Client(
                SupabaseSettings.Url,
                SupabaseSettings.PublishableKey,
                options);

            builder.Services.AddSingleton(client);
            builder.Services.AddSingleton<ISupabaseClientProvider, SupabaseClientProvider>();
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

#if ANDROID
            EditorHandler.Mapper.AppendToMapping("DiagnosticsEditor", (handler, view) =>
            {
                var editor = handler.PlatformView;

                // Multiline, but don't wrap lines horizontally
                editor.SetSingleLine(false);
                editor.SetHorizontallyScrolling(true);

                // Prevent Android from introducing its own line wrapping
                editor.SetMaxLines(int.MaxValue);
                editor.Ellipsize = null;

                // Use the simplest text layout
                editor.BreakStrategy = BreakStrategy.Simple;

                // Enable scrolling
                editor.VerticalScrollBarEnabled = true;
                editor.HorizontalScrollBarEnabled = true;
            });
#endif
            var app = builder.Build();

            var logService =
                app.Services.GetRequiredService<ILogService>();

            var dialogService =
                app.Services.GetRequiredService<IDialogService>();

            MethodLoggingService.SetExceptionHandler(
                async exception =>
                {
                    try
                    {
                        await MainThread.InvokeOnMainThreadAsync(
                            async () =>
                            {
                                await dialogService.ShowException(exception);
                            });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(
                            $"Exception displaying error dialog: {ex}");
                    }
                });
            
            logService.ClearAsync();

            MethodLoggingService.Configure(logService);

            return app;
        }
    }
}
