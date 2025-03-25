using ARSounds.UI.FontIcons;
using Microsoft.Maui.Controls;
using System;

namespace ARSounds.UI.Controls;

public partial class Rating : ContentView
{
    private readonly string RATING_EMPTY_ICON_CHAR = MaterialDesignIcons.StarBorder;
    private readonly string RATING_PARTIAL_ICON_CHAR = MaterialDesignIcons.StarHalf;
    private readonly string RATING_FULL_ICON_CHAR = MaterialDesignIcons.Star;
    public Rating()
    {
        InitializeComponent();
    }

    public static BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(double),
                typeof(Rating),
                0.0,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (Rating)bindable;
                    ctrl.RatingLabel.Text = ctrl.Update();

                }
            );

    public double Value
    {
        get { return (double)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static BindableProperty MaxProperty =
        BindableProperty.Create(
            nameof(Max),
            typeof(double),
            typeof(Rating),
            5.0,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var ctrl = (Rating)bindable;
                ctrl.RatingLabel.Text = ctrl.Update();
            }
        );

    public double Max
    {
        get { return (double)GetValue(MaxProperty); }
        set { SetValue(MaxProperty, value); }
    }

    public string Update()
    {
        var str = string.Empty;
        var value = Value;

        if (Value > Max)
        {
            value = Max;
        }
        else if (Value < 0)
        {
            value = 0;
        }

        for (var i = 1; i <= Max; i++)
        {
            if (i < value || Math.Abs((double)i - value) < 0.01) // i <= value
            {
                str += RATING_FULL_ICON_CHAR;
            }
            else
            {
                if (i - value > 1.0)
                {
                    str += RATING_EMPTY_ICON_CHAR;
                }
                else
                {
                    var decimals = value - Math.Floor(value);

                    if (decimals < 0.2)
                    {
                        str += RATING_EMPTY_ICON_CHAR;
                    }
                    else if (decimals > 0.8)
                    {
                        str += RATING_FULL_ICON_CHAR;
                    }
                    else
                    {
                        str += RATING_PARTIAL_ICON_CHAR;
                    }
                }
            }
        }

        return str;
    }
}