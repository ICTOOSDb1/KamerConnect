using KamerConnect.View.MAUI.UpdateAccountInputs;

using Microsoft.Maui.Controls;

namespace KamerConnect.View.MAUI
{
    public partial class UpdateAccount : ContentPage
    {
        public UpdateAccount()
        {
            InitializeComponent();
        }

        private void AccountDetails(object sender, EventArgs e)
        {
            FormsContainer.Content = new UpdateAccountsInput();
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesInputs();
        }
        
        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsInputs();
        }

        private void Other(object sender, EventArgs e)
        {
              FormsContainer.Content = new OtherInputs();
        }
    }
}
