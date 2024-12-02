using KamerConnect.Models;
using KamerConnect.View.MAUI.Components;

namespace KamerConnect.View.MAUI;

public partial class PersonalInformationForm : ContentView
{
    public PersonalInformationForm()
    {
        InitializeComponent();
        BindingContext = this;
    }
    public string? Email => emailEntry.Text;
    public string? FirstName => firstNameEntry.Text;
    public string? MiddleName => middleNameEntry.Text;
    public string? Surname => surnameEntry.Text;
    public string? PhoneNumber => phoneNumberEntry.Text;
    public string Password => passwordEntry.Text;
    public string ConfirmPassword => confirmPasswordEntry.Text;
    public DateTime? BirthDate => birthDateEntry.Text != null ? DateTime.Parse(birthDateEntry.Text) : null;
    public PickerOptions.DutchGender Gender => (PickerOptions.DutchGender)genderPicker.SelectedValue;
    

    public bool ValidateAll()
    {
        emailEntry?.Validate();
        firstNameEntry?.Validate();
        surnameEntry?.Validate();
        phoneNumberEntry?.Validate();
        birthDateEntry?.Validate();
        passwordEntry?.Validate();
        
        if (!string.IsNullOrEmpty(ConfirmPassword) && Password != ConfirmPassword)
        {
            confirmPasswordEntry.SetValidation("Wachtwoord komt niet overeen."); 
        }
        
        return (emailEntry?.IsValid ?? true) &&
               (firstNameEntry?.IsValid ?? true) &&
               (surnameEntry?.IsValid ?? true) &&
               (phoneNumberEntry?.IsValid ?? true) &&
               (passwordEntry?.IsValid ?? true) &&
               (Password == ConfirmPassword) &&
               (birthDateEntry?.IsValid ?? true);
    }
}