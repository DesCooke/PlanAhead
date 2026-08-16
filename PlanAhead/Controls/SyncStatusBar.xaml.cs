using PlanAhead.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace PlanAhead.Controls;

public partial class SyncStatusBar : ContentView
{
    public SyncStatusBar()
    {
        InitializeComponent();

        BindingContext = Application.Current!
                       .Handler!
                       .MauiContext!
                       .Services
                       .GetRequiredService<SyncStatusBarViewModel>(); 
    }

    protected override async void OnParentSet()
    {
        base.OnParentSet();

//        if (BindingContext is SyncStatusBarViewModel vm)
//            await vm.RefreshAsync();
    }
}