using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace KamerConnect.View.MAUI
{
    public partial class UpdateAccount : ContentPage
    {
        private FileService _fileService;

        public UpdateAccount(FileService fileService)
        {
            InitializeComponent();
            _fileService = fileService;
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
    }
}
