using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Targets.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Targets.Views;

public partial class TargetDetailsPage : ContentPage
{
    public TargetDetailsPage(TargetDetailsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}