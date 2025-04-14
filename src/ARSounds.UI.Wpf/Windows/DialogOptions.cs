using System.Windows;
using ARSounds.UI.Common.Windows;

namespace ARSounds.UI.Wpf.Windows;

public class DialogOptions
{
    #region Properties

    public double MinWidth { get; set; }

    public double MinHeight { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double MaxWidth { get; set; } = double.PositiveInfinity;

    public double MaxHeight { get; set; } = double.PositiveInfinity;

    public SizeToContent SizeToContent { get; set; }

    public string? WindowTitle { get; set; }

    public string? AnimationTitle { get; set; }

    public string? AnimationMessage { get; set; }

    public List<PluginButton> PluginButtons { get; } = [];

    public ResizeMode ResizeMode { get; set; }

    public bool IsTitleBarVisible { get; set; } = true;

    #endregion
}