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
        Text,
        Number
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

                case EntryInputType.DateOfBirth:
                    entry.Placeholder = "Select your date of birth";
                    entry.Keyboard = Keyboard.Numeric;
                    entry.LabelText = "Date of Birth";
                    entry.AddTapGestureRecognizerToOpenDatePicker();
                    break;

                case EntryInputType.PhoneNumber:
                    entry.Keyboard = Keyboard.Telephone;
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
}