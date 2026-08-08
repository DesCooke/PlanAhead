using PlanAhead.Resources.Icons;

namespace PlanAhead.Controls;

public partial class ObjectIcon : ContentView
{
    public ObjectIcon()
    {
        InitializeComponent();

        UpdateImageSource();
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