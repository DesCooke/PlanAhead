using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Services;

namespace PlanAhead
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();

            MainPage = shell;
        }
    }
}