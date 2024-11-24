using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI
{
    public partial class UpdateAccount : ContentPage
    {
        private FileService _fileService;
        private HouseService _houseService;

        public UpdateAccount(FileService fileService, HouseService houseService)
        {
            InitializeComponent();
            _fileService = fileService;
            _houseService = houseService;
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
            FormsContainer.Content = new HouseForm(_fileService, _houseService);
        }
    }
}
