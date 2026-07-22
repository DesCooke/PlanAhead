using CommunityToolkit.Mvvm.ComponentModel;
using PlanAhead.Core.Models.Enums;

public partial class FundEditViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private Frequency frequency;

    [ObservableProperty]
    private string notes = "";
}