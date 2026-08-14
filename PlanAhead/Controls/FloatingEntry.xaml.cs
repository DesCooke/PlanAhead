namespace PlanAhead.Controls;

public partial class FloatingEntry : ContentView
{
    private bool _isEntryFocused;
    private bool _showPassword;

    public bool IsPasswordVisible => _showPassword;

    private void EyeButton_Clicked(object sender, EventArgs e)
    {
        _showPassword = !_showPassword;

        EntryControl.IsPassword = !_showPassword;

        EyeButton.Source = _showPassword
            ? "eye_open.png"
            : "eye_closed.png";
    }

    public FloatingEntry()
    {
        InitializeComponent();

        // Standard appearance
        Margin = new Thickness(0, 0, 0, 20);
        HorizontalOptions = LayoutOptions.Fill;

        CardPadding = new Thickness(20);

        UpdateFloatingLabel();
    }


    // ============================================================
    // Label
    // ============================================================

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(
            nameof(Label),
            typeof(string),
            typeof(FloatingEntry),
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
            typeof(FloatingEntry),
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
            typeof(FloatingEntry),
            string.Empty);


    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }


    // ============================================================
    // Keyboard
    // ============================================================

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(
            nameof(Keyboard),
            typeof(Keyboard),
            typeof(FloatingEntry),
            Keyboard.Default);


    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
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

    private static void OnTextChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (FloatingEntry)bindable;

        control.UpdateFloatingLabel();
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
        UpdateFloatingLabel();
    }


    private void Entry_Unfocused(
        object sender,
        FocusEventArgs e)
    {
        _isEntryFocused = false;

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

        InputBorder.Stroke = _isEntryFocused
            ? Color.FromArgb("#6C4CE8")
            : Application.Current!.RequestedTheme == AppTheme.Light
                ? Color.FromArgb("#D0D0D0")
                : Color.FromArgb("#505050");
    }

    // ============================================================
    // IsPassword
    // ============================================================

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(FloatingEntry),
            false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    // ============================================================
    // Floating label
    // ============================================================

    private void UpdateFloatingLabel()
    {
        if (HintLabel == null || FloatingLabelBorder == null)
            return;

        bool shouldFloat =
            _isEntryFocused ||
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