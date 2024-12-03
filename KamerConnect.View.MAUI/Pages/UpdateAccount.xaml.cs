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
        private readonly HousePreferenceService _housePreferenceService;
        private Person _person;

        public UpdateAccount(FileService fileService, HouseService houseService, HousePreferenceService housePreferenceService,
            AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            _fileService = fileService;
            _houseService = houseService;
            _authenticationService = authenticationService;
            _personService = personService;
            _housePreferenceService = housePreferenceService;

            GetCurrentPerson().GetAwaiter().GetResult();
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _person);
        }

        private async Task GetCurrentPerson()
        {
            var session = await _authenticationService.GetSession();
            if (session != null)
            {
                _person = _personService.GetPersonById(session.personId);

                if (_person.Role == Role.Offering)
                {
                    HomePreferencesButton.IsVisible = false;
                }
                else if (_person.Role == Role.Seeking)
                {
                    HouseButton.IsVisible = false;
                }
            }
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
            FormsContainer.Content = new RegisterHomePreferencesForm(_housePreferenceService, _person);
            SetButtonStyles(HomePreferencesButton);
        }

        private void House(object sender, EventArgs e)
        {
            SetButtonStyles(HouseButton);
            FormsContainer.Content = new HouseForm(_fileService, _houseService, _person);
        }

        private void Button_Update(object sender, EventArgs e)
        {
            switch (FormsContainer.Content){
                case RegisterHomePreferencesForm form:
                    UpdateHousePreferences(form);
                    break;
                case InterestsForm form:
                    UpdateInterests(form);
                    break;
                case HouseForm form:
                    UpdateHouse(form);
                    break;
                case UpdateAccountsForm form:
                    UpdatePersonalInformation(form);
                    break;
            }
        }

        private async void UpdateHousePreferences(RegisterHomePreferencesForm form)
        {
            form.Button_Update_house_preferences();
        }
        private async void UpdateInterests(InterestsForm form)
        {
            form.Button_update_interests();
        }
        private void UpdateHouse(HouseForm form)
        {
            form.OnPublish();
        }
        private void UpdatePersonalInformation(UpdateAccountsForm form)
        {
            form.Button_Update_Account();
        }
        private void SetButtonStyles(Button buttonSource)
        {
            InterestsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            AccountDetailsButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            HomePreferencesButton.Style = (Style)Application.Current.Resources["SecondaryButton"];
            HouseButton.Style = (Style)Application.Current.Resources["SecondaryButton"];

            buttonSource.Style = (Style)Application.Current.Resources["PrimaryButton"];
        }
    }
}
