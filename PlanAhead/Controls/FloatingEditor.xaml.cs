namespace PlanAhead.Controls;

public partial class FloatingEditor : ContentView
{
    private bool _isEditorFocused;


    public FloatingEditor()
    {
        InitializeComponent();

        // Standard appearance
        Margin = new Thickness(0, 0, 0, 20);
        HorizontalOptions = LayoutOptions.Fill;

        MinimumHeight = 100;
        MaximumHeight = 200;

        UpdateFloatingLabel();
    }


    // ============================================================
    // Label
    // ============================================================

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(
            nameof(Label),
            typeof(string),
            typeof(FloatingEditor),
            string.Empty);

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    // ============================================================
    // Text
    // ============================================================

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FloatingEditor),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: OnTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    // ============================================================
    // Placeholder
    // ============================================================

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(FloatingEditor),
            string.Empty);


    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }


    // ============================================================
    // Minimum Height
    // ============================================================

    public static readonly BindableProperty MinimumHeightProperty =
        BindableProperty.Create(
            nameof(MinimumHeight),
            typeof(double),
            typeof(FloatingEditor),
            100.0);


    public double MinimumHeight
    {
        get => (double)GetValue(MinimumHeightProperty);
        set => SetValue(MinimumHeightProperty, value);
    }


    // ============================================================
    // Maximum Height
    // ============================================================

    public static readonly BindableProperty MaximumHeightProperty =
        BindableProperty.Create(
            nameof(MaximumHeight),
            typeof(double),
            typeof(FloatingEditor),
            200.0);


    public double MaximumHeight
    {
        get => (double)GetValue(MaximumHeightProperty);
        set => SetValue(MaximumHeightProperty, value);
    }


    // ============================================================
    // Text changed
    // ============================================================

    private static void OnTextChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (FloatingEditor)bindable;

        control.UpdateFloatingLabel();
    }


    // ============================================================
    // Focus
    // ============================================================

    private void Editor_Focused(
        object sender,
        FocusEventArgs e)
    {
        _isEditorFocused = true;

        UpdateBorder();
        UpdateFloatingLabel();
    }


    private void Editor_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        _isEditorFocused = false;

        UpdateBorder();
        UpdateFloatingLabel();
    }


    // ============================================================
    // Border
    // ============================================================

    private void UpdateBorder()
    {
        if (InputBorder == null)
            return;

        InputBorder.Stroke = _isEditorFocused
            ? Color.FromArgb("#6C4CE8")
            : Application.Current!.RequestedTheme == AppTheme.Light
                ? Color.FromArgb("#D0D0D0")
                : Color.FromArgb("#505050");
    }


    // ============================================================
    // Floating label
    // ============================================================

    private void UpdateFloatingLabel()
    {
        if (HintLabel == null || FloatingLabelBorder == null)
            return;

        bool shouldFloat =
            _isEditorFocused ||
            !string.IsNullOrWhiteSpace(Text);

        if (shouldFloat)
        {
            HintLabel.Opacity = 0;

            FloatingLabelBorder.Opacity = 1;
            FloatingLabelBorder.TranslationY = 0;
        }
        else
        {
            FloatingLabelBorder.Opacity = 0;
            FloatingLabelBorder.TranslationY = 0;

            HintLabel.Opacity = 1;
        }
    }
}