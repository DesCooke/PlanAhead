using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.Constants;

namespace HomeBudget.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = AppConstants.ApplicationName;

    [ObservableProperty]
    private string welcomeMessage =
        "Welcome to Home Budget";

    [ObservableProperty]
    private string version =
        $"Version {AppConstants.Version}";
}