using PlanAhead.Core.Models.Enums;
using System.Windows.Input;
using System.Diagnostics;

namespace PlanAhead.Controls;

public partial class LargeOptionButton : ContentView
{
    public LargeOptionButton()
    {
        InitializeComponent();
    }


    // ------------------------------------------------------------
    // Icon
    // ------------------------------------------------------------

    public static readonly BindableProperty IconIdProperty =
        BindableProperty.Create(
            nameof(IconId),
            typeof(string),
            typeof(LargeOptionButton),
            default(string),
            propertyChanged: OnDisplayPropertyChanged);

    public string? IconId
    {
        get => (string?)GetValue(IconIdProperty);
        set => SetValue(IconIdProperty, value);
    }


    public bool HasIcon =>
        !string.IsNullOrWhiteSpace(IconId);


    // ------------------------------------------------------------
    // Status
    // ------------------------------------------------------------

    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(
            nameof(Status),
            typeof(Status),
            typeof(LargeOptionButton),
            Status.NotSet,
            propertyChanged: OnStatusChanged);

    public Status Status
    {
        get => (Status)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    private static void OnStatusChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (LargeOptionButton)bindable;

        System.Diagnostics.Debug.WriteLine(
            $"LargeOptionButton Status: {oldValue} -> {newValue}");
    }
    // ------------------------------------------------------------
    // Title
    // ------------------------------------------------------------

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(LargeOptionButton),
            default(string),
            propertyChanged: OnDisplayPropertyChanged);

    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    public bool HasTitle =>
        !string.IsNullOrWhiteSpace(Title);


    // ------------------------------------------------------------
    // Subtitle
    // ------------------------------------------------------------

    public static readonly BindableProperty SubtitleProperty =
        BindableProperty.Create(
            nameof(Subtitle),
            typeof(string),
            typeof(LargeOptionButton),
            default(string),
            propertyChanged: OnDisplayPropertyChanged);

    public string? Subtitle
    {
        get => (string?)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }


    public bool HasSubtitle =>
        !string.IsNullOrWhiteSpace(Subtitle);


    // ------------------------------------------------------------
    // Detail
    // ------------------------------------------------------------

    public static readonly BindableProperty DetailProperty =
        BindableProperty.Create(
            nameof(Detail),
            typeof(string),
            typeof(LargeOptionButton),
            default(string),
            propertyChanged: OnDisplayPropertyChanged);

    public string? Detail
    {
        get => (string?)GetValue(DetailProperty);
        set => SetValue(DetailProperty, value);
    }


    public bool HasDetail =>
        !string.IsNullOrWhiteSpace(Detail);



    // ------------------------------------------------------------
    // Property changed
    // ------------------------------------------------------------

    private static void OnDisplayPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (LargeOptionButton)bindable;

        control.OnPropertyChanged(nameof(HasIcon));
        control.OnPropertyChanged(nameof(HasTitle));
        control.OnPropertyChanged(nameof(HasSubtitle));
        control.OnPropertyChanged(nameof(HasDetail));
    }

    public static readonly BindableProperty OpenCommandProperty =
        BindableProperty.Create(
            nameof(OpenCommand),
            typeof(ICommand),
            typeof(LargeOptionButton),
            null,
            propertyChanged: OnOpenCommandChanged);

    private static void OnOpenCommandChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        System.Diagnostics.Debug.WriteLine($"OpenCommand = {newValue}");
    }
    public ICommand? OpenCommand
    {
        get => (ICommand?)GetValue(OpenCommandProperty);
        set => SetValue(OpenCommandProperty, value);
    }

}