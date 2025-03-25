using ARSounds.UI.Maui.Targets.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Targets.Views;

public partial class TargetsListPage : ContentPage
{
    public TargetsListPage(TargetsListViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}