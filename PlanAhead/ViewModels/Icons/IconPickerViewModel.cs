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

    public void Initialise(string? currentIconId)
    {
        SelectedIcon = Icons.FirstOrDefault(i => i.Id == currentIconId);
    }

    [ObservableProperty]
    private IconDefinitionViewModel? selectedIcon;

    partial void OnSelectedIconChanged(IconDefinitionViewModel? value)
    {
        // This will fire whenever the user taps an icon.
    }
}