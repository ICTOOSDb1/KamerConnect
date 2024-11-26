using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Maui.Controls;

namespace KamerConnect.View.MAUI.Pages
{
    public partial class UpdateAccount : ContentPage
    {
        private FileService _fileService;
        private PersonService _personService;
        private Person _currentPerson;
        
        public Person mockPerson = new Person(
            email: "john.doe@example.com",
            firstName: "John",
            middleName: "A",
            surname: "Doe",
            phoneNumber: "+1234567890",
            birthDate: new DateTime(1990, 5, 15),
            gender: Gender.Male,
            role: Role.Seeking,
            profilePicturePath: "http://localhost:9000/profilepictures/klassendiagram.png",
            id: "3483a37c-f810-4d82-90e3-8d78d372f4ad"
        );
        public UpdateAccount(FileService fileService, PersonService personService)
        {
            _fileService = fileService;
            _personService = personService;
            _currentPerson = mockPerson;
            InitializeComponent();
            if (_currentPerson.Role == Role.Offering)
            {
                HomePreferencesButton.IsVisible = false;
            }
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, mockPerson);
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, mockPerson);
            SetButtonStyles(AccountDetailsButton);
        }
        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsForm(_personService, _currentPerson);
            SetButtonStyles(InterestsButton);
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesForm(_personService, _currentPerson);
            SetButtonStyles(HomePreferencesButton);
        }
        
        private void SocialMedia(object sender, EventArgs e)
        {
            FormsContainer.Content = new SocialMediaForm(_personService, _currentPerson);
            SetButtonStyles(SocialMediaButton);
        }


        private void SetButtonStyles(Button buttonSource)
        {
            InterestsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            AccountDetailsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            HomePreferencesButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            SocialMediaButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            
            buttonSource.Style = (Style)Application.Current.Resources["PrimaryButton"];
        }
    }
}
