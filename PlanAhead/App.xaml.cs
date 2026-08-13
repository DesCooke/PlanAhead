using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage();   // temporary blank page
        }

        protected override async void OnStart()
        {
            var startup =
                Handler.MauiContext.Services.GetRequiredService<IApplicationStartupService>();

            MainPage = await startup.GetStartupPageAsync();
        }
    }
}