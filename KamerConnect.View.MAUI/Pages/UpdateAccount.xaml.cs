using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI
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
            InitializeComponent();
            _fileService = fileService;
            _houseService = houseService;
            _authenticationService = authenticationService;
            _personService = personService;

            GetCurrentPerson();
        }

        private async void GetCurrentPerson()
        {
            var session = await _authenticationService.GetSession();
            if (session != null)
                _person = _personService.GetPersonById(session.personId);
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsForm(_fileService);
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesForm();
        }

        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsForm();
        }

        private void House(object sender, EventArgs e)
        {
            FormsContainer.Content = new HouseForm(_fileService, _houseService, _person);
        }
    }
}
