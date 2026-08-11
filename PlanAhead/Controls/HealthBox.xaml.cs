using PlanAhead.Core.Models.Enums;

namespace PlanAhead.Controls;

public partial class HealthBox : ContentView
{
    public HealthBox()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty HealthProperty =
        BindableProperty.Create(
            nameof(Health),
            typeof(Status),
            typeof(HealthBox),
            Status.Green,
            propertyChanged: OnHealthChanged);

    public Status Health
    {
        get => (Status)GetValue(HealthProperty);
        set => SetValue(HealthProperty, value);
    }

    public static readonly BindableProperty HealthTextProperty =
        BindableProperty.Create(
            nameof(HealthText),
            typeof(string),
            typeof(HealthBox),
            "Healthy");

    public string HealthText
    {
        get => (string)GetValue(HealthTextProperty);
        set => SetValue(HealthTextProperty, value);
    }

    public static readonly BindableProperty HealthColourProperty =
        BindableProperty.Create(
            nameof(HealthColour),
            typeof(Color),
            typeof(HealthBox),
            Colors.Green);

    public Color HealthColour
    {
        get => (Color)GetValue(HealthColourProperty);
        set => SetValue(HealthColourProperty, value);
    }


    private static void OnHealthChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (HealthBox)bindable;

        control.UpdateHealth();
    }

    private void UpdateHealth()
    {
        switch (Health)
        {
            case Status.Green:
                HealthText = "Healthy";
                HealthColour = Colors.Green;
                break;

            case Status.Amber:
                HealthText = "Warning";
                HealthColour = Colors.Orange;
                break;

            case Status.Red:
                HealthText = "Critical";
                HealthColour = Colors.Red;
                break;
        }
    }
}