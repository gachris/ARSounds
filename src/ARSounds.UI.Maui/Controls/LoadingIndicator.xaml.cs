namespace ARSounds.UI.Maui.Controls;

public partial class LoadingIndicator : Border
{
    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
        nameof(IsBusy),
        typeof(bool),
        typeof(LoadingIndicator),
        false,
        BindingMode.OneWay,
        null,
        SetIsBusy);

    public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
        nameof(LoadingText),
        typeof(string),
        typeof(LoadingIndicator),
        string.Empty,
        BindingMode.OneWay,
        null,
        SetLoadingText);

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public string LoadingText
    {
        get => (string)GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }

    public LoadingIndicator()
    {
        InitializeComponent();
    }

    private static void SetIsBusy(BindableObject bindable, object oldValue, object newValue)
    {
        var loadingIndicator = (LoadingIndicator)bindable;
        loadingIndicator.IsVisible = (bool)newValue;
        loadingIndicator.actIndicator.IsRunning = (bool)newValue;
    }

    private static void SetLoadingText(BindableObject bindable, object oldValue, object newValue)
    {
        var loadingIndicator = (LoadingIndicator)bindable;
        loadingIndicator.lblLoadingText.Text = (string)newValue;
    }
}
