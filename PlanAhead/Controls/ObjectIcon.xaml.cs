using PlanAhead.Core.Models.Enums;
using PlanAhead.Resources.Icons;

namespace PlanAhead.Controls;

public partial class ObjectIcon : ContentView
{
    public ObjectIcon()
    {
        InitializeComponent();

        UpdateImageSource();
    }

    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(
            nameof(Status),
            typeof(Status),
            typeof(ObjectIcon),
            Status.NotSet,
            propertyChanged: OnStatusChanged);

    public Status Status
    {
        get => (Status)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public bool HasStatus =>
        Status != Status.NotSet;

    private static void OnStatusChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (ObjectIcon)bindable;

        control.HealthColor = newValue switch
        {
            Status.Green => Colors.Green,
            Status.Amber => Colors.Orange,
            Status.Red => Colors.Red,
            _ => Colors.Transparent
        };

        control.OnPropertyChanged(nameof(HasStatus));
    }
    public static readonly BindableProperty HealthColorProperty =
        BindableProperty.Create(
            nameof(HealthColor),
            typeof(Color),
            typeof(ObjectIcon),
            Colors.Green);

    public Color HealthColor
    {
        get => (Color)GetValue(HealthColorProperty);
        private set => SetValue(HealthColorProperty, value);
    }

    public static readonly BindableProperty IconIdProperty =
        BindableProperty.Create(
            nameof(IconId),
            typeof(string),
            typeof(ObjectIcon),
            "PiggyBank",
            propertyChanged: OnIconIdChanged);

    public string IconId
    {
        get => (string)GetValue(IconIdProperty);
        set => SetValue(IconIdProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(
            nameof(IconSize),
            typeof(double),
            typeof(ObjectIcon),
            28.0);


    public double ButtonSize
    {
        get => (double)GetValue(ButtonSizeProperty);
        set => SetValue(ButtonSizeProperty, value);
    }

    public static readonly BindableProperty ButtonSizeProperty =
        BindableProperty.Create(
            nameof(ButtonSize),
            typeof(double),
            typeof(ObjectIcon),
            60.0);

    public int ButtonPadding
    {
        get => (int)GetValue(ButtonPaddingProperty);
        set => SetValue(ButtonPaddingProperty, value);
    }

    public static readonly BindableProperty ButtonPaddingProperty =
        BindableProperty.Create(
            nameof(ButtonPadding),
            typeof(int),
            typeof(ObjectIcon),
            16);

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }


    private static void OnIconIdChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        ((ObjectIcon)bindable).UpdateImageSource();
    }

    private void UpdateImageSource()
    {
        var resourceName = IconCatalogue.GetResourceName(IconId);

        IconImage.Source = ImageSource.FromFile($"{resourceName}.png");
    }
}