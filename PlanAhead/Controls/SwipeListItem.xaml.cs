using PlanAhead.Core.Models.Enums;
using System.Windows.Input;

namespace PlanAhead.Controls;

public partial class SwipeListItem : ContentView
{
    public SwipeListItem()
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
            typeof(SwipeListItem),
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
            typeof(SwipeListItem),
            Status.NotSet);

    public Status Status
    {
        get => (Status)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    // ------------------------------------------------------------
    // Title
    // ------------------------------------------------------------

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(SwipeListItem),
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
            typeof(SwipeListItem),
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
            typeof(SwipeListItem),
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
    // Edit
    // ------------------------------------------------------------

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(SwipeListItem),
            propertyChanged: OnCommandChanged);
    public ICommand? EditCommand
    {
        get => (ICommand?)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }


    public static readonly BindableProperty EditCommandParameterProperty =
        BindableProperty.Create(
            nameof(EditCommandParameter),
            typeof(object),
            typeof(SwipeListItem));

    public object? EditCommandParameter
    {
        get => GetValue(EditCommandParameterProperty);
        set => SetValue(EditCommandParameterProperty, value);
    }


    public bool HasEditCommand =>
        EditCommand != null;


    // ------------------------------------------------------------
    // Delete
    // ------------------------------------------------------------

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(SwipeListItem),
            propertyChanged: OnCommandChanged);

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }


    public static readonly BindableProperty DeleteCommandParameterProperty =
        BindableProperty.Create(
            nameof(DeleteCommandParameter),
            typeof(object),
            typeof(SwipeListItem));

    public object? DeleteCommandParameter
    {
        get => GetValue(DeleteCommandParameterProperty);
        set => SetValue(DeleteCommandParameterProperty, value);
    }


    public bool HasDeleteCommand =>
        DeleteCommand != null;


    // ------------------------------------------------------------
    // Property changed
    // ------------------------------------------------------------

    private static void OnDisplayPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (SwipeListItem)bindable;

        control.OnPropertyChanged(nameof(HasIcon));
        control.OnPropertyChanged(nameof(HasTitle));
        control.OnPropertyChanged(nameof(HasSubtitle));
        control.OnPropertyChanged(nameof(HasDetail));
    }

    private static void OnCommandChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (SwipeListItem)bindable;

        control.OnPropertyChanged(nameof(HasEditCommand));
        control.OnPropertyChanged(nameof(HasDeleteCommand));
    }
    public static readonly BindableProperty OpenCommandProperty =
        BindableProperty.Create(
            nameof(OpenCommand),
            typeof(ICommand),
            typeof(SwipeListItem));

    public ICommand? OpenCommand
    {
        get => (ICommand?)GetValue(OpenCommandProperty);
        set => SetValue(OpenCommandProperty, value);
    }

    public static readonly BindableProperty OpenCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpenCommandParameter),
            typeof(object),
            typeof(SwipeListItem));

    public object? OpenCommandParameter
    {
        get => GetValue(OpenCommandParameterProperty);
        set => SetValue(OpenCommandParameterProperty, value);
    }
}