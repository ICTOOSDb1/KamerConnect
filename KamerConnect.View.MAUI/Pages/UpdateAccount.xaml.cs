using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI
{
    public partial class UpdateAccount : ContentPage
    {
        private FileService _fileService;

        public UpdateAccount(FileService fileService)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
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
