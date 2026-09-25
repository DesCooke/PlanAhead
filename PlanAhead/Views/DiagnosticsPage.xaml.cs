using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PlanAhead.Views;

public partial class DiagnosticsPage : ContentPage
{
    private DiagnosticsViewModel? _viewModel;

    public DiagnosticsPage(DiagnosticsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DiagnosticsViewModel.Log))
        {
            MainThread.BeginInvokeOnMainThread(
                async () => await RefreshLogDisplay());
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Give the page time to complete its initial layout.
        await Task.Yield();

        // Load the current log.
        await _viewModel.InitialiseAsync();

        // Render it.
        await RefreshLogDisplay();
    }

    private async Task RefreshLogDisplay()
    {
        if (_viewModel == null)
            return;

        var log = _viewModel.Log;

        LogStack.Children.Clear();

        if (string.IsNullOrEmpty(log))
            return;

        var lines = log.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.None);

        foreach (var line in lines)
        {
            LogStack.Children.Add(CreateLogLine(line));
        }

        // Give MAUI a chance to lay out the content.
        await Task.Yield();

        // Start horizontally at the left.
        await LogScrollView.ScrollToAsync(0, 0, false);
    }

    private Label CreateLogLine(string text)
    {
        var label = new Label
        {
            FontFamily = "CourierNew",
            FontSize = 12,
            LineBreakMode = LineBreakMode.NoWrap,
            HorizontalOptions = LayoutOptions.Start,
            TextColor = Color.FromArgb("#E8EAED"),
            Padding = new Thickness(0, 1)
        };

        label.FormattedText = ParseLogText(text);

        return label;
    }

    private FormattedString ParseLogText(string text)
    {
        var formatted = new FormattedString();

        var pattern = @"<(Bold|Green|Red)>(.*?)</\1>";

        var matches = Regex.Matches(
            text,
            pattern,
            RegexOptions.Singleline |
            RegexOptions.IgnoreCase);

        var position = 0;

        foreach (Match match in matches)
        {
            // Normal text before the tag.
            if (match.Index > position)
            {
                formatted.Spans.Add(
                    new Span
                    {
                        Text = text[position..match.Index],
                        TextColor = Color.FromArgb("#E8EAED")
                    });
            }

            var tag = match.Groups[1].Value;
            var content = match.Groups[2].Value;

            var span = new Span
            {
                Text = content,
                TextColor = Color.FromArgb("#E8EAED")
            };

            switch (tag.ToLowerInvariant())
            {
                case "bold":
                    span.FontAttributes = FontAttributes.Bold;
                    span.TextColor = Color.FromArgb("#FFFFFF");
                    break;

                case "green":
                    span.TextColor = Color.FromArgb("#81C995");
                    break;

                case "red":
                    span.TextColor = Colors.Red;
                    break;
            }

            formatted.Spans.Add(span);

            position = match.Index + match.Length;
        }

        // Text after the last tag.
        if (position < text.Length)
        {
            formatted.Spans.Add(
                new Span
                {
                    Text = text[position..],
                    TextColor = Color.FromArgb("#E8EAED")
                });
        }

        // Important when the line contains no tags.
        if (formatted.Spans.Count == 0)
        {
            formatted.Spans.Add(
                new Span
                {
                    Text = text,
                    TextColor = Color.FromArgb("#E8EAED")
                });
        }

        return formatted;
    }
}