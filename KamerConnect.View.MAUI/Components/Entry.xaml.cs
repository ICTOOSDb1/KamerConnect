using KamerConnect.Utils;

namespace KamerConnect.View.MAUI.Components;

public partial class Entry : ContentView
{
    public enum EntryInputType
    {
        Email,
        Password,
        ConfirmPassword,
        Date,
        PhoneNumber,
        Text,
        Number,
        Decimal,
        PostalCode
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
                    entry.Keyboard = Keyboard.Email;
                    entry.LabelText = "E-mail";
                    break;

                case EntryInputType.Password:
                    entry.Keyboard = Keyboard.Text;
                    entry.IsPassword = true;
                    entry.LabelText = "Wachtwoord";
                    break;

                case EntryInputType.ConfirmPassword:
                    entry.Keyboard = Keyboard.Text;
                    entry.IsPassword = true;
                    entry.LabelText = "Wachtwoord bevestigen";
                    break;

                case EntryInputType.Date:
                    entry.Keyboard = Keyboard.Numeric;
                    break;

                case EntryInputType.PhoneNumber:
                    entry.Keyboard = Keyboard.Numeric;
                    entry.LabelText = "Telefoon nummer";
                    break;

                case EntryInputType.Number:
                    entry.Keyboard = Keyboard.Numeric;
                    break;

                default:
                    entry.Keyboard = Keyboard.Default;
                    entry.IsPassword = false;
                    break;
            }
        }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(Entry), string.Empty);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(Entry), false);

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }



    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Text) && IsRequired)
        {
            SetValidation("dit veld is verplicht");
        }
        else if (InputType == EntryInputType.Email && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsValidEmail(Text))
        {
            SetValidation("Emailadres is ongeldig.");
        }
        else if (InputType == EntryInputType.PhoneNumber && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsValidPhoneNumber(Text))
        {
            SetValidation("Telefoonnummer is ongeldig.");
        }
        else if (InputType == EntryInputType.Date && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsValidDate(Text))
        {
            SetValidation("Geboorte datum is ongeldig.");
        }
        else if (InputType == EntryInputType.PostalCode && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsValidPostalCode(Text))
        {
            SetValidation("Postcode is ongeldig.");
        }
        else if (InputType == EntryInputType.Number && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsInteger(Text))
        {
            SetValidation("Geen geldig nummer.");
        }
        else if (InputType == EntryInputType.Decimal && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsDouble(Text))
        {
            SetValidation("Geen geldig nummer.");
        }
        else if (InputType == EntryInputType.Password && !string.IsNullOrEmpty(Text) && !ValidationUtils.IsValidPassword(Text))
        {
            SetValidation("Wachtwoord moet minimaar 8 tekens hebben.");
        }
        else
        {
            IsValid = true;
            hideLabel();
            ValidationMessage = string.Empty;
        }
    }

    public static readonly BindableProperty IsLabelVisibleProperty =
        BindableProperty.Create(nameof(IsLabelVisible), typeof(bool), typeof(Entry), false);

    public bool IsLabelVisible
    {
        get => (bool)GetValue(IsLabelVisibleProperty);
        set => SetValue(IsLabelVisibleProperty, value);
    }

    public void SetValidation(string message)
    {
        IsValid = false;
        ValidationMessage = message;
        inputNotCorrect.Text = ValidationMessage;
        showLabel();
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