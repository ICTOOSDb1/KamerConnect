using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Components;

public partial class Entry : ContentView
{
    public enum EntryInputType
    {
        Email, 
        Password,
        DateOfBirth,
        PhoneNumber,
        Text
    }
    
    public Entry()
    {
        InitializeComponent();
    }

    
    public static readonly BindableProperty InputTypeProperty =
        BindableProperty.Create(nameof(InputType), typeof(EntryInputType), typeof(Entry), EntryInputType.Text, propertyChanged: OnInputTypeChanged);

    public EntryInputType InputType
    {
        get => (EntryInputType)GetValue(InputTypeProperty);
        set => SetValue(InputTypeProperty, value);
    }
    
    private static void OnInputTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Entry entry && newValue is EntryInputType inputType)
        {
            switch (inputType)
            {
                case EntryInputType.Email:
                    entry.Placeholder = "Voer uw e-mailadres in";
                    entry.Keyboard = Keyboard.Email;
                    entry.LabelText = "E-mail";
                    break;

                case EntryInputType.Password:
                    entry.Placeholder = "Voer uw wachtwoord in";
                    entry.Keyboard = Keyboard.Text;
                    entry.IsPassword = true;
                    entry.LabelText = "Wachtwoord";
                    break;

                case EntryInputType.DateOfBirth:
                    entry.Placeholder = "Voer uw geboortedatum in";
                    entry.Keyboard = Keyboard.Numeric;
                    entry.LabelText = "Geboortedatum";
                    break;
                case EntryInputType.PhoneNumber:
                    entry.Placeholder = "voor uw telefoon nummer in";
                    entry.Keyboard = Keyboard.Telephone;
                    entry.LabelText = "Telefoon nummer";
                    break;

                default:
                    entry.Keyboard = Keyboard.Default;
                    entry.IsPassword = false;
                    break;
            }
        }
    }
    
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Entry), string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(Entry), Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(Entry), false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
                                  
        
    public static readonly BindableProperty LabelTextProperty = 
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(Entry), default(string));

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

}