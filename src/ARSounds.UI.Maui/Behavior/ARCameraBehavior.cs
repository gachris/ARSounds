using System.Windows.Input;
using ARSounds.UI.Common.Data;
using OpenVision.Core.Reco;
using OpenVision.Maui.Controls;

namespace ARSounds.UI.Maui.Behaviors;

public class ARCameraBehavior : Behavior<ARCamera>
{
    #region Fields/Consts

    private ARCamera? _camera;

    public static readonly BindableProperty ClientApiKeyProperty =
        BindableProperty.Create(
            nameof(ClientApiKey),
            typeof(string),
            typeof(ARCameraBehavior),
            null,
            propertyChanged: OnClientApiKeyChanged);

    public static readonly BindableProperty TrackFoundCommandProperty =
        BindableProperty.Create(
            nameof(TrackFoundCommand),
            typeof(ICommand),
            typeof(ARCameraBehavior),
            null);

    public static readonly BindableProperty TrackLostCommandProperty =
        BindableProperty.Create(
            nameof(TrackLostCommand),
            typeof(ICommand),
            typeof(ARCameraBehavior),
            null);

    #endregion

    #region Properties

    public string ClientApiKey
    {
        get => (string)GetValue(ClientApiKeyProperty);
        set => SetValue(ClientApiKeyProperty, value);
    }

    public ICommand? TrackFoundCommand
    {
        get => (ICommand?)GetValue(TrackFoundCommandProperty);
        set => SetValue(TrackFoundCommandProperty, value);
    }

    public ICommand? TrackLostCommand
    {
        get => (ICommand?)GetValue(TrackLostCommandProperty);
        set => SetValue(TrackLostCommandProperty, value);
    }

    #endregion

    #region Methods Overrides

    protected override void OnAttachedTo(ARCamera bindable)
    {
        base.OnAttachedTo(bindable);

        _camera = bindable;

        if (_camera == null)
            throw new InvalidOperationException("ARCameraBehavior must be attached to an ARCamera control.");

        BindingContext = _camera.BindingContext;
        _camera.BindingContextChanged += OnBindingContextChanged;

        _camera.Loaded += Camera_Loaded;
        _camera.TrackFound += Camera_TrackFound;
        _camera.TrackLost += Camera_TrackLost;

        InitializeCloudRecognition();
    }

    protected override void OnDetachingFrom(ARCamera bindable)
    {
        base.OnDetachingFrom(bindable);

        if (_camera != null)
        {
            _camera.Loaded -= Camera_Loaded;
            _camera.TrackFound -= Camera_TrackFound;
            _camera.TrackLost -= Camera_TrackLost;
            _camera = null;
        }
    }

    #endregion

    #region Methods

    private static void OnClientApiKeyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var behavior = (ARCameraBehavior)bindable;
        behavior.InitializeCloudRecognition();
    }

    private async void InitializeCloudRecognition()
    {
        if (string.IsNullOrWhiteSpace(ClientApiKey) || _camera == null)
            return;

        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(ClientApiKey);

        _camera.SetRecoService(cloudRecognition);
    }

    #endregion

    #region Events Subscriptions

    private void Camera_Loaded(object? sender, EventArgs e)
    {
        InitializeCloudRecognition();
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        BindingContext = _camera?.BindingContext;
    }

    private void Camera_TrackFound(object? sender, TargetMatchingEventArgs e)
    {
        var result = new TargetMatchingResult(e.Frame, e.TargetMatchResults);
        if (TrackFoundCommand?.CanExecute(result) == true)
        {
            TrackFoundCommand.Execute(result);
        }
    }

    private void Camera_TrackLost(object? sender, EventArgs e)
    {
        if (TrackLostCommand?.CanExecute(null) == true)
        {
            TrackLostCommand.Execute(null);
        }
    }

    #endregion
}