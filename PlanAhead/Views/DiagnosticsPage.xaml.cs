using PlanAhead.ViewModels;
using System.ComponentModel;

namespace PlanAhead.Views;

public partial class DiagnosticsPage : ContentPage
{
    private readonly DiagnosticsViewModel _viewModel;

    public DiagnosticsPage(DiagnosticsViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;

		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitialiseAsync();

        if (BindingContext is DiagnosticsViewModel viewModel)
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

#if ANDROID
        Dispatcher.Dispatch(() =>
        {
            if (LogEditor.Handler?.PlatformView is Android.Widget.EditText nativeEditor)
            {
                nativeEditor.Post(() =>
                {
                    nativeEditor.SetSelection(0);
                    nativeEditor.ScrollTo(0, nativeEditor.ScrollY);
                });
            }
        });
#endif
    }

    protected override void OnDisappearing()
    {
        if (BindingContext is DiagnosticsViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        base.OnDisappearing();
    }

    private void ViewModel_PropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DiagnosticsViewModel.Log))
            return;

#if ANDROID
        if (LogEditor.Handler?.PlatformView is Android.Widget.EditText nativeEditor)
        {
            nativeEditor.Post(() =>
            {
                nativeEditor.SetSelection(0);
                nativeEditor.ScrollTo(0, nativeEditor.ScrollY);
            });
        }
#endif
    }

    private void LogEditor_Loaded(object? sender, EventArgs e)
    {
#if ANDROID
        if (LogEditor.Handler?.PlatformView is Android.Widget.EditText nativeEditor)
        {
            nativeEditor.Post(() =>
            {
                nativeEditor.SetSelection(0);
                nativeEditor.ScrollTo(0, nativeEditor.ScrollY);
            });
        }
#endif
    }
    private void LogEditor_TextChanged(object? sender, TextChangedEventArgs e)
    {
#if ANDROID
        if (sender is Editor editor &&
            editor.Handler?.PlatformView is Android.Widget.EditText nativeEditor)
        {
            nativeEditor.Post(() =>
            {
                nativeEditor.SetSelection(0);
                nativeEditor.ScrollTo(0, nativeEditor.ScrollY);
            });
        }
#endif
    }
}