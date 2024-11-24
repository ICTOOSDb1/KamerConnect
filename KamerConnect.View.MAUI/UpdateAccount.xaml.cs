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
        
        public UpdateAccount(FileService fileService, PersonService personService, Person currentPerson)
        {
            _fileService = fileService;
            _personService = personService;
            InitializeComponent();
            _currentPerson = currentPerson;
            if (_currentPerson.Role == Role.Offering)
            {
                HomePreferencesButton.IsVisible = false;
            }
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsForm(_fileService, _personService, _currentPerson);
        }
        
        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsForm(_personService, _currentPerson);
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesForm();
        }
    }
}
