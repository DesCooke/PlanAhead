namespace PlanAhead.Controls;

public partial class FloatingCheckbox : ContentView
{
    private bool _isEntryFocused;

    public FloatingCheckbox()
    {
        InitializeComponent();

        // Standard appearance
        Margin = new Thickness(0, 0, 0, 20);
        HorizontalOptions = LayoutOptions.Fill;

        CardPadding = new Thickness(20);
    }


    // ============================================================
    // Label
    // ============================================================

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(
            nameof(Label),
            typeof(string),
            typeof(FloatingCheckbox),
            string.Empty);

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    // ============================================================
    // Text
    // ============================================================

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(CheckBox),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnCheckboxChanged);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }


    // ============================================================
    // Placeholder
    // ============================================================

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(FloatingEntry),
            string.Empty);


    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }



    // ============================================================
    // Card Padding
    // ============================================================

    public static readonly BindableProperty CardPaddingProperty =
        BindableProperty.Create(
            nameof(CardPadding),
            typeof(Thickness),
            typeof(FloatingEntry),
            new Thickness(20));


    public Thickness CardPadding
    {
        get => (Thickness)GetValue(CardPaddingProperty);
        set => SetValue(CardPaddingProperty, value);
    }


    // ============================================================
    // Text changed
    // ============================================================

    private static void OnCheckboxChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (FloatingCheckbox)bindable;
    }


    // ============================================================
    // Focus
    // ============================================================

    private void Entry_Focused(
        object sender,
        FocusEventArgs e)
    {
        _isEntryFocused = true;

        UpdateBorder();
    }


    private void Entry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        _isEntryFocused = false;

        UpdateBorder();
    }


    // ============================================================
    // Border
    // ============================================================

    private void UpdateBorder()
    {
        if (InputBorder == null)
            return;

        InputBorder.Stroke = _isEntryFocused
            ? Color.FromArgb("#6C4CE8")
            : Application.Current!.RequestedTheme == AppTheme.Light
                ? Color.FromArgb("#D0D0D0")
                : Color.FromArgb("#505050");
    }


}