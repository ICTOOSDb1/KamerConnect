namespace KamerConnect.View.MAUI;

public partial class PersonalInformationForm : ContentView
{
    public PersonalInformationForm()
    {
        InitializeComponent();
    }
    public string? Email => emailEntry.Text;
    public string? FirstName => firstNameEntry.Text;
    public string? MiddleName => middleNameEntry.Text;
    public string? Surname => surnameEntry.Text;
    public string? PhoneNumber => phoneNumberEntry.Text;
    public string Password => passwordEntry.Text;
    public DateTime? BirthDate => birthDateEntry.Text != null ? DateTime.Parse(birthDateEntry.Text) : null;
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
        passwordEntry.Validate();
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