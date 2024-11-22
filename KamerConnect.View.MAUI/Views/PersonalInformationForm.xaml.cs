namespace KamerConnect.View.MAUI;

public partial class PersonalInformationForm : ContentView
{
    public PersonalInformationForm()
    {
        InitializeComponent();
    }
    public string Email => emailEntry?.DefaultText ?? string.Empty;
    public string FirstName => firstNameEntry?.DefaultText ?? string.Empty;
    public string MiddleName => string.IsNullOrWhiteSpace(middleNameEntry?.DefaultText) ? null : middleNameEntry.DefaultText;
    public string Surname => surnameEntry?.DefaultText ?? string.Empty;
    public string PhoneNumber => string.IsNullOrWhiteSpace(phoneNumberEntry?.DefaultText) ? null : phoneNumberEntry.DefaultText;

    public DateTime BirthDate => (birthDateEntry == null) ? DateTime.Parse(birthDateEntry?.DefaultText ?? DateTime.MinValue.ToString("yyyy-MM-dd")) : DateTime.Today;
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