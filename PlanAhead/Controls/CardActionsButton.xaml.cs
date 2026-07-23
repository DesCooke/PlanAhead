using System.Windows.Input;

namespace PlanAhead.Controls;

public partial class CardActionsButton : ContentView
{
    public CardActionsButton()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty EditCommandProperty =
        BindableProperty.Create(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(CardActionsButton));

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(CardActionsButton));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(CardActionsButton));

    public ICommand? EditCommand
    {
        get => (ICommand?)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private async void Button_Clicked(object? sender, EventArgs e)
    {
        var action =
            await Application.Current!.MainPage!
                .DisplayActionSheet(
                    "Choose Action",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");

        switch (action)
        {
            case "Edit":
                if (EditCommand?.CanExecute(CommandParameter) == true)
                    EditCommand.Execute(CommandParameter);
                break;

            case "Delete":
                if (DeleteCommand?.CanExecute(CommandParameter) == true)
                    DeleteCommand.Execute(CommandParameter);
                break;
        }
    }
}