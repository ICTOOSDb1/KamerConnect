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
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.Navigation.PushAsync(new Registration());
        }
    }
}