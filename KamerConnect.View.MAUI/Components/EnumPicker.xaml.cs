using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Components;

public partial class EnumPicker : ContentView
{
    public EnumPicker()
    {
        InitializeComponent();
        BindingContext = this;
    }
    
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(EnumPicker), string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly BindableProperty SelectedEnumValueProperty =
        BindableProperty.Create(nameof(SelectedEnumValue), typeof(Enum), typeof(EnumPicker), null, propertyChanged: OnSelectedEnumValueChanged);

    public Enum SelectedEnumValue
    {
        get { return (Enum)GetValue(SelectedEnumValueProperty); }
        set { SetValue(SelectedEnumValueProperty, value); }
    }
    
    public List<KeyValuePair<Enum, string>> EnumOptions { get; set; }
    
    public void SetEnumType(Type enumType, Dictionary<Enum, string> translations)
    {
        if (enumType == null || !enumType.IsEnum)
            throw new ArgumentException("Provided type is not an enum");
        
        EnumOptions = Enum.GetValues(enumType).Cast<Enum>()
            .Select(e => new KeyValuePair<Enum, string>(e, translations.ContainsKey(e) ? translations[e] : e.ToString()))
            .ToList();
        
        enumPicker.ItemsSource = EnumOptions;
    }
    
    private static void OnSelectedEnumValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var picker = (EnumPicker)bindable;
        if (newValue != null)
        {
            picker.SelectedEnumValue = (Enum)newValue;
        }
    }
}

