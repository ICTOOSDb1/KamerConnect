using KamerConnect.View.MAUI.Views;

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
            FormsContainer.Content = new UpdateAccountsForm();
        }

        private void HomePreferences(object sender, EventArgs e)
        {
            FormsContainer.Content = new HomePreferencesForm();
        }
        
        private void Interests(object sender, EventArgs e)
        {
            FormsContainer.Content = new InterestsForm();
        }

        private void Other(object sender, EventArgs e)
        {
              FormsContainer.Content = new OtherForm();
        }
    }
}
