using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Maui.Controls;

namespace KamerConnect.View.MAUI
{
    public partial class UpdateAccount : ContentPage
    {
        private FileService _fileService;
        private PersonService _personService;
        private Person _currentPerson;
        
        public UpdateAccount(FileService fileService, PersonService personService, Person person)
        {
            _fileService = fileService;
            _personService = personService;
            _currentPerson = person;
            InitializeComponent();
            if (_currentPerson.Role == Role.Offering)
            {
                HomePreferencesButton.IsVisible = false;
            }
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _currentPerson);
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _currentPerson);
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
