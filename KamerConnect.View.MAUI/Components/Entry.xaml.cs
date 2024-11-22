using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;

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
                    entry.Placeholder = "Select your date of birth";
                    entry.Keyboard = Keyboard.Numeric; // Optional, but typically not used for dates
                    entry.LabelText = "Date of Birth";
                    entry.AddTapGestureRecognizerToOpenDatePicker();
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
    private void AddTapGestureRecognizerToOpenDatePicker()
    {
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            // Temporary DatePicker for selecting date
            var datePicker = new DatePicker
            {
                IsVisible = false // Keep it hidden
            };

            // Add DatePicker to the parent layout
            var parent = this.Parent as Layout<Microsoft.Maui.Controls.View>;
            if (parent != null)
            {
                parent.Children.Add(datePicker);
                datePicker.Focus(); // Focus on the DatePicker to open the dialog

                datePicker.DateSelected += (sender, args) =>
                {
                    DefaultText = args.NewDate.ToString("yyyy-MM-dd"); // Set selected date as text
                    parent.Children.Remove(datePicker); // Remove DatePicker after selection
                };
            }
        };

        this.GestureRecognizers.Add(tapGestureRecognizer);
    }
    
    public static readonly BindableProperty DefaultTextProperty =
        BindableProperty.Create(nameof(DefaultText), typeof(string), typeof(Entry), string.Empty);
    public string DefaultText
    {
        get => (string)GetValue(DefaultTextProperty);
        set => SetValue(DefaultTextProperty, value);
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