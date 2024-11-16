using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace KamerConnect.View.MAUI.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public ICommand NavigateToRegisterCommand { get; }

    public LoginViewModel()
    {
        NavigateToRegisterCommand = new Command(NavigateToRegister);
    }

    private async void NavigateToRegister()
    {
        // Ensure the Navigation property is set in the code-behind or elsewhere
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.Navigation.PushAsync(new Register());
        }
    }
}