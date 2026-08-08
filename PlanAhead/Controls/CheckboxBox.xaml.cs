namespace PlanAhead.Controls;

public partial class CheckboxBox : ContentView
{
	public CheckboxBox()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextLabelProperty =
        BindableProperty.Create(
            nameof(TextLabel),
            typeof(string),
            typeof(CheckboxBox),
            "<FieldLabel>");

    public string TextLabel
    {
        get => (string)GetValue(TextLabelProperty);
        set => SetValue(TextLabelProperty, value);
    }

    public static readonly BindableProperty CheckboxProperty =
        BindableProperty.Create(
            nameof(Checkbox),
            typeof(bool),
            typeof(CheckboxBox),
            false);

    public bool Checkbox
    {
        get => (bool)GetValue(CheckboxProperty);
        set => SetValue(CheckboxProperty, value);
    }

    public int ButtonPadding
    {
        get => (int)GetValue(ButtonPaddingProperty);
        set => SetValue(ButtonPaddingProperty, value);
    }

    public static readonly BindableProperty ButtonPaddingProperty =
        BindableProperty.Create(
            nameof(ButtonPadding),
            typeof(int),
            typeof(CheckboxBox),
            16);

}