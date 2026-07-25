using CommunityToolkit.Mvvm.ComponentModel;
using PlanAhead.Resources.Icons;

namespace PlanAhead.ViewModels.Icons;

public partial class IconDefinitionViewModel : ObservableObject
{
    public IconDefinitionViewModel(IconDefinition icon)
    {
        Icon = icon;
    }

    public IconDefinition Icon { get; }

    public string Id => Icon.Id;

    public string DisplayName => Icon.DisplayName;

    public string ResourceName => Icon.ResourceName;

    [ObservableProperty]
    private bool isSelected;
}