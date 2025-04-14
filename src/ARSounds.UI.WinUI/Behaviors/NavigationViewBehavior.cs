using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace ARSounds.UI.WinUI.Behaviors;

public class NavigationViewBehavior : Behavior<NavigationView>
{
    #region Fields/Consts

    public static readonly DependencyProperty IsSettingsSelectedProperty =
        DependencyProperty.RegisterAttached(nameof(IsSettingsSelected), typeof(bool), typeof(NavigationViewBehavior), new PropertyMetadata(false, OnIsSettingsSelectedChanged));

    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.RegisterAttached(nameof(SelectedValue), typeof(object), typeof(NavigationViewBehavior), new PropertyMetadata(null, OnSelectedValueChanged));

    public static readonly DependencyProperty SelectedValuePathProperty =
        DependencyProperty.RegisterAttached(nameof(SelectedValuePath), typeof(string), typeof(NavigationViewBehavior), new PropertyMetadata(null, OnSelectedValuePathChanged));
    private bool _skipSelectedValueChanged;

    #endregion

    #region Properties

    public bool IsSettingsSelected
    {
        get => (bool)GetValue(IsSettingsSelectedProperty);
        set => SetValue(IsSettingsSelectedProperty, value);
    }

    public object? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public string SelectedValuePath
    {
        get => (string)GetValue(SelectedValuePathProperty);
        set => SetValue(SelectedValuePathProperty, value);
    }

    #endregion

    #region Methods Overrides

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;

        UpdateSelectedValue();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
    }

    #endregion

    #region Methods

    private static void OnIsSettingsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var navigationViewBehavior = (NavigationViewBehavior)d;
        navigationViewBehavior.OnIsSettingsSelectedChanged(e);
    }

    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var navigationViewBehavior = (NavigationViewBehavior)d;
        navigationViewBehavior.OnSelectedValueChanged(e);
    }

    private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var navigationViewBehavior = (NavigationViewBehavior)d;
        navigationViewBehavior.OnSelectedValuePathChanged(e);
    }

    private void OnIsSettingsSelectedChanged(DependencyPropertyChangedEventArgs e)
    {
        var isSettingsSelected = (bool)e.NewValue;

        if (isSettingsSelected)
        {
            AssociatedObject.SelectedItem = AssociatedObject.SettingsItem;
        }
    }

    private void OnSelectedValueChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_skipSelectedValueChanged || AssociatedObject is null)
        {
            return;
        }

        UpdateSelectedItem();
    }

    private void OnSelectedValuePathChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdateSelectedValue();
    }

    private void UpdateSelectedValue()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var selectedItem = AssociatedObject.SelectedItem;

        if (!string.IsNullOrEmpty(SelectedValuePath))
        {
            var selectedValue = selectedItem?.GetType().GetProperty(SelectedValuePath)?.GetValue(selectedItem, null);
            SelectedValue = selectedValue;
        }
        else
        {
            SelectedValue = selectedItem;
        }
    }

    private void UpdateSelectedItem()
    {
        if (IsSettingsSelected)
        {
            return;
        }

        if (!string.IsNullOrEmpty(SelectedValuePath))
        {
            foreach (var item in AssociatedObject.MenuItems)
            {
                var selectedValue = item.GetType().GetProperty(SelectedValuePath)?.GetValue(item, null);

                if (selectedValue != null && selectedValue.Equals(SelectedValue))
                {
                    AssociatedObject.SelectedItem = item;
                    return;
                }
            }

            AssociatedObject.SelectedItem = null;
        }
        else
        {
            AssociatedObject.SelectedItem = SelectedValue;
        }
    }

    #endregion

    #region Events Subscriptions

    private void AssociatedObject_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        IsSettingsSelected = args.IsSettingsSelected;

        _skipSelectedValueChanged = true;

        UpdateSelectedValue();

        _skipSelectedValueChanged = false;
    }

    #endregion
}
