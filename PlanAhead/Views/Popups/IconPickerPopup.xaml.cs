using CommunityToolkit.Maui.Views;
using PlanAhead.Views.Popups;
using PlanAhead.ViewModels.Icons;

namespace PlanAhead.Views.Popups;

public partial class IconPickerPopup : Popup
{
    public IconPickerPopup(IconPickerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}