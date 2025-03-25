using ARSounds.UI.Maui.Common.ViewModels;
using ARSounds.UI.Maui.Targets.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Targets.Views;

public partial class TargetDetailsPage : ContentPage
{
    public TargetDetailsPage(TargetDetailsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}