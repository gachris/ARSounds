using System.Windows;
using System.Windows.Input;
using ARSounds.UI.Common.Data;
using Microsoft.Xaml.Behaviors;
using OpenVision.Core.Reco;
using OpenVision.Wpf.Controls;

namespace ARSounds.UI.Wpf.Behaviors;

public class ARCameraBehavior : Behavior<ARCamera>
{
    #region Fields/Consts

    public static readonly DependencyProperty ClientApiKeyProperty =
        DependencyProperty.Register(nameof(ClientApiKey), typeof(string), typeof(ARCameraBehavior), new PropertyMetadata(null, OnClientApiKeyChanged));

    public static readonly DependencyProperty TrackFoundCommandProperty =
        DependencyProperty.Register(nameof(TrackFoundCommand), typeof(ICommand), typeof(ARCameraBehavior), new PropertyMetadata(null));

    public static readonly DependencyProperty TrackLostCommandProperty =
        DependencyProperty.Register(nameof(TrackLostCommand), typeof(ICommand), typeof(ARCameraBehavior), new PropertyMetadata(null));

    #endregion

    #region Properties

    public string ClientApiKey
    {
        get => (string)GetValue(ClientApiKeyProperty);
        set => SetValue(ClientApiKeyProperty, value);
    }

    public ICommand TrackFoundCommand
    {
        get => (ICommand)GetValue(TrackFoundCommandProperty);
        set => SetValue(TrackFoundCommandProperty, value);
    }

    public ICommand TrackLostCommand
    {
        get => (ICommand)GetValue(TrackLostCommandProperty);
        set => SetValue(TrackLostCommandProperty, value);
    }

    #endregion

    public ARCameraBehavior()
    {
    }

    #region Methods Overrides

    protected override void OnAttached()
    {
        base.OnAttached();

        InitializeCloudRecognition();

        AssociatedObject.Loaded += AssociatedObject_Loaded;
        AssociatedObject.TrackFound += AssociatedObject_TrackFound;
        AssociatedObject.TrackLost += AssociatedObject_TrackLost;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.TrackFound -= AssociatedObject_TrackFound;
            AssociatedObject.TrackLost -= AssociatedObject_TrackLost;
        }

        base.OnDetaching();
    }

    #endregion

    #region Methods

    private static void OnClientApiKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var arCameraBehavior = (ARCameraBehavior)d;
        arCameraBehavior.OnClientApiKeyChanged(e);
    }

    private void OnClientApiKeyChanged(DependencyPropertyChangedEventArgs _)
    {
        InitializeCloudRecognition();
    }

    private async void InitializeCloudRecognition()
    {
        if (string.IsNullOrWhiteSpace(ClientApiKey) || AssociatedObject == null)
            return;

        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(ClientApiKey);

        AssociatedObject.SetRecoService(cloudRecognition);
    }

    #endregion

    #region Events Subscriptions

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeCloudRecognition();
    }

    private void AssociatedObject_TrackFound(object? sender, TargetMatchingEventArgs e)
    {
        var result = new TargetMatchingResult(e.Frame, e.TargetMatchResults);
        if (TrackFoundCommand?.CanExecute(result) == true)
        {
            TrackFoundCommand.Execute(result);
        }
    }

    private void AssociatedObject_TrackLost(object? sender, EventArgs e)
    {
        if (TrackLostCommand?.CanExecute(null) == true)
        {
            TrackLostCommand.Execute(null);
        }
    }

    #endregion
}
