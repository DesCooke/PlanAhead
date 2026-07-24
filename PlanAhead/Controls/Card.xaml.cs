namespace PlanAhead.Controls;

public partial class Card : ContentView
{
    public Card()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CardMarginProperty =
        BindableProperty.Create(
            nameof(CardMargin),
            typeof(Thickness),
            typeof(Card),
            new Thickness(10, 5));

    public Thickness CardMargin
    {
        get => (Thickness)GetValue(CardMarginProperty);
        set => SetValue(CardMarginProperty, value);
    }

    public static readonly BindableProperty CardPaddingProperty =
        BindableProperty.Create(
            nameof(CardPadding),
            typeof(Thickness),
            typeof(Card),
            new Thickness(16));

    public Thickness CardPadding
    {
        get => (Thickness)GetValue(CardPaddingProperty);
        set => SetValue(CardPaddingProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(Card),
            Colors.LightGray);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BorderThicknessProperty =
        BindableProperty.Create(
            nameof(BorderThickness),
            typeof(double),
            typeof(Card),
            1.0);

    public double BorderThickness
    {
        get => (double)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }
}