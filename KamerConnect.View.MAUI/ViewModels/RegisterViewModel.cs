using System.Windows.Input;

namespace KamerConnect.View.MAUI.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public ICommand SelectTabCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand SubmitCommand { get; }

    private string _selectedTab;
    public string SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (SetProperty(ref _selectedTab, value))
            {
                UpdateSubmitButtonText();
            }
        }
    }

    private string _submitButtonText;
    public string SubmitButtonText
    {
        get => _submitButtonText;
        set => SetProperty(ref _submitButtonText, value);
    }
    
    private string _huisButtonColor = "#EF626C";
    public string HuisButtonColor
    {
        get => _huisButtonColor;
        set => SetProperty(ref _huisButtonColor, value);
    }

    private string _huisgenootButtonColor = "#FFFFFF";
    public string HuisgenootButtonColor
    {
        get => _huisgenootButtonColor;
        set => SetProperty(ref _huisgenootButtonColor, value);
    }

    public RegisterViewModel()
    {
        SelectTabCommand = new Command<string>(SelectTab);
        GoBackCommand = new Command(GoBack);
        SubmitCommand = new Command(Submit);

        // Initialize with default values
        SelectedTab = "huis"; // Default tab
        UpdateSubmitButtonText();
    }

    private void SelectTab(string tab)
    {
        SelectedTab = tab;
        UpdateButtonColors();
    }
    
    private void UpdateButtonColors()
    {
        if (SelectedTab == "huis")
        {
            HuisButtonColor = "#EF626C"; // Gold for "ik zoek een huis"
            HuisgenootButtonColor = "#ffffff"; // Light Gray for "ik zoek een huisgenoot"
        }
        else
        {
            HuisButtonColor = "#ffffff"; // Light Gray for "ik zoek een huis"
            HuisgenootButtonColor = "#EF626C"; // Gold for "ik zoek een huisgenoot"
        }
    }

    private void UpdateSubmitButtonText()
    {
        SubmitButtonText = SelectedTab == "huis"
            ? "verder gaan als huiszoekende"
            : "verder gaan als huisgenootzoekende";
    }

    private void GoBack()
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PopAsync();
        }
    }

    private void Submit()
    {
        Console.WriteLine($"Form submitted for {SelectedTab}!");
    }
}
