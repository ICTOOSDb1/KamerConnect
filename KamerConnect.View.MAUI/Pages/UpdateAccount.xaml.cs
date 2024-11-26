using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Pages
{
    public partial class UpdateAccount : ContentPage
    {
        private readonly FileService _fileService;
        private readonly HouseService _houseService;
        private readonly PersonService _personService;
        private readonly AuthenticationService _authenticationService;
        private Person _person;

        public UpdateAccount(FileService fileService, HouseService houseService,
        AuthenticationService authenticationService, PersonService personService)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            _fileService = fileService;
            _houseService = houseService;
            _authenticationService = authenticationService;
            _personService = personService;

            GetCurrentPerson();
            if (_person.Role == Role.Offering)
            {
                HomePreferencesButton.IsVisible = false;
            }
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _person);
        }

        private async void GetCurrentPerson()
        {
            var session = await _authenticationService.GetSession();
            if (session != null)
                _person = _personService.GetPersonById(session.personId);
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _person);
            SetButtonStyles(AccountDetailsButton);
        }
        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsForm(_personService, _person);
            SetButtonStyles(InterestsButton);
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesForm(_personService, _person);
            SetButtonStyles(HomePreferencesButton);
        }

        private void SocialMedia(object sender, EventArgs e)
        {
            FormsContainer.Content = new SocialMediaForm(_personService, _person);
            SetButtonStyles(SocialMediaButton);
            FormsContainer.Content = new RegisterHomePreferencesForm();
        }


        private void SetButtonStyles(Button buttonSource)
        {
            InterestsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            AccountDetailsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            HomePreferencesButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            SocialMediaButton.Style = (Style)Application.Current.Resources["SecondaryButton"];

            buttonSource.Style = (Style)Application.Current.Resources["PrimaryButton"];
        }

        private void House(object sender, EventArgs e)
        {
            FormsContainer.Content = new HouseForm(_fileService, _houseService, _person);
        }
    }
}
