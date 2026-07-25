using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanAhead.Resources.Icons;
using System.Collections.ObjectModel;

namespace PlanAhead.ViewModels.Icons;

public partial class IconPickerViewModel : ObservableObject
{
    public ObservableCollection<IconDefinitionViewModel> Icons { get; } = [];

    public IconPickerViewModel()
    {
        foreach (var icon in IconCatalogue.All)
        {
            Icons.Add(new IconDefinitionViewModel(icon));
        }
    }

    [ObservableProperty]
    private IconDefinitionViewModel? selectedIcon;
}