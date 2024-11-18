using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Components;

public partial class CustomCheckbox : ContentView
{
    public CustomCheckbox()
    {
        InitializeComponent();
    }
    public static readonly BindableProperty LabelTextProperty = 
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(CustomCheckbox), default(string));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    // Bindable property for the checkbox state
    public static readonly BindableProperty IsCheckedProperty = 
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomCheckbox), default(bool), BindingMode.TwoWay);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
}