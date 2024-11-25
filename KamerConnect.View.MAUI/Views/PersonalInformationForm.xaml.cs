namespace KamerConnect.View.MAUI;

public partial class PersonalInformationForm : ContentView
{
    public PersonalInformationForm()
    {
        InitializeComponent();
    }
    public string? Email => emailEntry.DefaultText;
    public string? FirstName => firstNameEntry.DefaultText;
    public string? MiddleName => middleNameEntry.DefaultText;
    public string? Surname => surnameEntry.DefaultText;
    public string? PhoneNumber => phoneNumberEntry.DefaultText;
    public DateTime? BirthDate => birthDateEntry.DefaultText != null ? DateTime.Parse(birthDateEntry.DefaultText) : null;
    public string Gender
    {
        get
        {
            if (maleRadioButton.IsChecked) return "Male";
            if (femaleRadioButton.IsChecked) return "Female";
            if (otherRadioButton.IsChecked) return "Other";
            return "Other";
        }
    }

    public bool ValidateAll()
    {
        emailEntry?.Validate();
        firstNameEntry?.Validate();
        surnameEntry?.Validate();
        phoneNumberEntry?.Validate();
        birthDateEntry?.Validate();
        
        bool isGenderValid = maleRadioButton.IsChecked || femaleRadioButton.IsChecked || otherRadioButton.IsChecked;
        if (!isGenderValid)
        {
            radiobuttonNotSelected.IsVisible = true;
        }
        else
        {
            radiobuttonNotSelected.IsVisible = false;
        }
        
        return (emailEntry?.IsValid ?? true) &&
               (firstNameEntry?.IsValid ?? true) &&
               (surnameEntry?.IsValid ?? true) &&
               (phoneNumberEntry?.IsValid ?? true) &&
               (birthDateEntry?.IsValid ?? true);
    }
}