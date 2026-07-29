using CommunityToolkit.Maui.Views;
using PlanAhead.ViewModels.Icons;
using PlanAhead.Views.Popups;
using System.ComponentModel;

namespace PlanAhead.Views.Popups;

public partial class IconPickerPopup : Popup
{
    private readonly IconPickerViewModel _viewModel;

    private bool _initialising;

    public string? SelectedIconId { get; private set; }

    public IconPickerPopup(IconPickerViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender,
        PropertyChangedEventArgs e)
    {
        if (_initialising)
            return;

        if (e.PropertyName == nameof(IconPickerViewModel.SelectedIcon)
            && _viewModel.SelectedIcon is not null)
        {
            SelectedIconId = _viewModel.SelectedIcon.Id;

            CloseAsync();
        }
    }

    public void Initialise(string? currentIconId)
    {
        _initialising = true;

        _viewModel.Initialise(currentIconId);

        _initialising = false;
    }

    private async void IconTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is IconDefinitionViewModel icon)
        {
            SelectedIconId = icon.Id;

            await CloseAsync();
        }
    }
}