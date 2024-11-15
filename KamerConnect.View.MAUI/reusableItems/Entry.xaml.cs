using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.reusableItems;

public partial class Entry : ContentView
{
    public Entry()
    {
        InitializeComponent();
    }
    public static readonly BindableProperty LabelTextProperty = 
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(Entry), default(string));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = 
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Entry), "Enter your password");

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty TextProperty = 
        BindableProperty.Create(nameof(Text), typeof(string), typeof(Entry), default(string), BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}