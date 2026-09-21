using Microsoft.Extensions.DependencyInjection;
using PlanAhead.Core.Interfaces.Services;
using System.Diagnostics;

namespace PlanAhead
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();

            MainPage = shell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Created += (_, _) =>
                Debug.WriteLine("=== WINDOW CREATED ===");

            window.Activated += (_, _) =>
                Debug.WriteLine("=== WINDOW ACTIVATED ===");

            window.Deactivated += (_, _) =>
                Debug.WriteLine("=== WINDOW DEACTIVATED ===");

            window.Stopped += (_, _) =>
                Debug.WriteLine("=== WINDOW STOPPED ===");

            window.Resumed += (_, _) =>
                Debug.WriteLine("=== WINDOW RESUMED ===");

            window.Destroying += (_, _) =>
                Debug.WriteLine("=== WINDOW DESTROYING ===");

            return window;
        }
    }
}