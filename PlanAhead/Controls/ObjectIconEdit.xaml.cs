using System.Windows.Input;

namespace PlanAhead.Controls;

public partial class ObjectIconEdit : ContentView
{
    public ObjectIconEdit()
    {
        InitializeComponent();

        Margin = new Thickness(0, 0, 0, 20);

        HorizontalOptions = LayoutOptions.Center;

        CardPadding = new Thickness(20);

        ButtonPadding = new Thickness(16);

        ButtonSize = 70;

        IconSize = 36;
    }

    public static readonly BindableProperty IconIdProperty =
        BindableProperty.Create(
            nameof(IconId),
            typeof(string),
            typeof(ObjectIconEdit),
            string.Empty);

    public string IconId
    {
        get => (string)GetValue(IconIdProperty);
        set => SetValue(IconIdProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(ObjectIconEdit),
            "Change Icon");

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(ObjectIconEdit));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(ObjectIconEdit));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty CardPaddingProperty =
        BindableProperty.Create(
            nameof(CardPadding),
            typeof(Thickness),
            typeof(ObjectIconEdit));

    public Thickness CardPadding
    {
        get => (Thickness)GetValue(CardPaddingProperty);
        set => SetValue(CardPaddingProperty, value);
    }

    public static readonly BindableProperty ButtonPaddingProperty =
        BindableProperty.Create(
            nameof(ButtonPadding),
            typeof(Thickness),
            typeof(ObjectIconEdit));

    public Thickness ButtonPadding
    {
        get => (Thickness)GetValue(ButtonPaddingProperty);
        set => SetValue(ButtonPaddingProperty, value);
    }

    public static readonly BindableProperty ButtonSizeProperty =
        BindableProperty.Create(
            nameof(ButtonSize),
            typeof(double),
            typeof(ObjectIconEdit));

    public double ButtonSize
    {
        get => (double)GetValue(ButtonSizeProperty);
        set => SetValue(ButtonSizeProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(
            nameof(IconSize),
            typeof(double),
            typeof(ObjectIconEdit));

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }
}