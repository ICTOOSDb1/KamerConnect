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
}