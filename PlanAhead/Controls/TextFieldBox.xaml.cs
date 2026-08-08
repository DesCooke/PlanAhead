using PlanAhead.Resources.Icons;

namespace PlanAhead.Controls;

public partial class TextFieldBox : ContentView
{
    public TextFieldBox()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextLabelProperty =
        BindableProperty.Create(
            nameof(TextLabel),
            typeof(string),
            typeof(TextFieldBox),
            "<FieldLabel>");

    public string TextLabel
    {
        get => (string)GetValue(TextLabelProperty);
        set => SetValue(TextLabelProperty, value);
    }

    public static readonly BindableProperty TextContentProperty =
        BindableProperty.Create(
            nameof(TextContent),
            typeof(string),
            typeof(TextFieldBox),
            "<FieldLabel>");

    public string TextContent
    {
        get => (string)GetValue(TextContentProperty);
        set => SetValue(TextContentProperty, value);
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
            typeof(TextFieldBox),
            16);

}