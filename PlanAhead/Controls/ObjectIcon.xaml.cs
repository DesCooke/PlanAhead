using PlanAhead.Resources.Icons;

namespace PlanAhead.Controls;

public partial class ObjectIcon : ContentView
{
    public ObjectIcon()
    {
        InitializeComponent();

        UpdateImageSource();
    }

    public static readonly BindableProperty IconNameProperty =
        BindableProperty.Create(
            nameof(IconName),
            typeof(string),
            typeof(ObjectIcon),
            "PiggyBank",
            propertyChanged: OnIconNameChanged);

    public string IconName
    {
        get => (string)GetValue(IconNameProperty);
        set => SetValue(IconNameProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(
            nameof(IconSize),
            typeof(double),
            typeof(ObjectIcon),
            28.0);

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    private static void OnIconNameChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        ((ObjectIcon)bindable).UpdateImageSource();
    }

    private void UpdateImageSource()
    {
        var resourceName = IconCatalogue.GetResourceName(IconName);

        IconImage.Source = ImageSource.FromFile($"{resourceName}.png");
    }
}