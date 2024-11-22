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
        BindingContext = this;
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
                    entry.Keyboard = Keyboard.Numeric;
                    entry.LabelText = "Date of Birth";
                    entry.AddTapGestureRecognizerToOpenDatePicker();
                    break;
                case EntryInputType.PhoneNumber:
                    entry.Placeholder = "voor uw telefoon nummer in";
                    entry.Keyboard = Keyboard.Numeric;
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
         
            var datePicker = new DatePicker
            {
                IsVisible = false 
            };
            
            var parent = this.Parent as Layout<Microsoft.Maui.Controls.View>;
            if (parent != null)
            {
                parent.Children.Add(datePicker);
                datePicker.Focus(); 

                datePicker.DateSelected += (sender, args) =>
                {
                    DefaultText = args.NewDate.ToString("yyyy-MM-dd");
                    parent.Children.Remove(datePicker);
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
    public static readonly BindableProperty ValidationMessageProperty =
        BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(Entry), string.Empty);

    public string ValidationMessage
    {
        get => (string)GetValue(ValidationMessageProperty);
        set => SetValue(ValidationMessageProperty, value);
    }

    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(Entry), true);

    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(DefaultText) && InputType!=EntryInputType.PhoneNumber)
        {
            IsValid = false;
            ValidationMessage = $"{LabelText} cannot be empty.";
            TestLabel.Text = ValidationMessage;
            showLabel();
        }
        else if (InputType == EntryInputType.Email && !IsValidEmail(DefaultText))
        {
            IsValid = false;
            ValidationMessage = "Please enter a valid email address.";
            TestLabel.Text = ValidationMessage;
            showLabel();
        }
        else if (InputType == EntryInputType.PhoneNumber && !IsNumeric(DefaultText) && !string.IsNullOrWhiteSpace(DefaultText))
        {
            IsValid = false;
            ValidationMessage = "Telefoonnummer is ongeldig.";
            TestLabel.Text = ValidationMessage;
            showLabel();
        }
        else if (InputType == EntryInputType.DateOfBirth && !IsNumeric(DefaultText))
        {
            IsValid = false;
            ValidationMessage = "geboorte datum is ongeldig.";
            TestLabel.Text = ValidationMessage;
            showLabel();
        }
        else
        {
            IsValid = true;
            hideLabel();
            ValidationMessage = string.Empty;
        }
    }

    private bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private bool IsNumeric(string text)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\d/]+$");
    }
    public static readonly BindableProperty IsLabelVisibleProperty =
        BindableProperty.Create(nameof(IsLabelVisible), typeof(bool), typeof(Entry), false);

    public bool IsLabelVisible
    {
        get => (bool)GetValue(IsLabelVisibleProperty);
        set => SetValue(IsLabelVisibleProperty, value);
    }
    public void showLabel()
    {
        IsLabelVisible = true;
    }
    public void hideLabel()
    {
        IsLabelVisible = false;
    }

}