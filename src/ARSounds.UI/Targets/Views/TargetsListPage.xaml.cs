using ARSounds.UI.Targets.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Targets.Views;

public partial class TargetsListPage : ContentPage
{
    public TargetsListPage(TargetsListViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}