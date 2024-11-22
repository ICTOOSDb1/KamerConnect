using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class Registration : ContentPage, INotifyPropertyChanged
{
	public Registration()
	{
		InitializeComponent();
        SelectTabAction = SelectTab;
        BindingContext = this;
        SelectedTab = "search house"; 
        UpdateButtonColors();
	}
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	
    public Action<string> SelectTabAction { get; set; }

	private async void Terug(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Count > 1)
		{
			await Navigation.PopAsync();
		}
		else
		{
		}
	}

	private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is RadioButton radioButton && e.Value)
		{

		}
	}
    
    private void OnHuisClicked(object sender, EventArgs e)
    {
        SelectTab("search house");
    }

    private void OnHuisgenootClicked(object sender, EventArgs e)
    {
        SelectTab("have a house");
        
    }

    private string _selectedTab;
    public string SelectedTab
    {
        get => _selectedTab;
        set => _selectedTab = value;
    }

    
    private string _huisButtonColor = "#EF626C";
    public string HuisButtonColor
    {
	    get => _huisButtonColor;
	    set
	    {
		    if (_huisButtonColor != value)
		    {
			    _huisButtonColor = value;
			    OnPropertyChanged();
		    }
	    }
    }

    private string _huisgenootButtonColor = "#FFFFFF";
    public string HuisgenootButtonColor
    {
	    get => _huisgenootButtonColor;
	    set
	    {
		    if (_huisgenootButtonColor != value)
		    {
			    _huisgenootButtonColor = value;
			    OnPropertyChanged();
		    }
	    }
    }
   

    private void SelectTab(string tab)
    {
        SelectedTab = tab;
        UpdateButtonColors();
        
    }
    
    private void UpdateButtonColors()
    {
        if (SelectedTab == "search house")
        {
            HuisButtonColor = "#EF626C"; 
            HuisgenootButtonColor = "#ffffff"; 
        }
        else
        {
            HuisButtonColor = "#ffffff";
            HuisgenootButtonColor = "#EF626C";
        }
    }
    private void CreatePerson()
    {
	    Role role;

	    if (SelectedTab == "search house")
	    {
		    role = Role.Seeking;
	    }
	    else
	    {
		    role = Role.Offering;
	    }

	    var newPerson = new Person(
		    personalInformationForm.Email,
		    personalInformationForm.FirstName,
		    personalInformationForm.MiddleName,
		    personalInformationForm.Surname,
		    personalInformationForm.PhoneNumber,
		    personalInformationForm.BirthDate,
		    Enum.Parse<Gender>(personalInformationForm.Gender ?? "Other"),
		    role,
		    null
	    );
    }

    private void submit(object? sender, EventArgs e)
    {
	    if (personalInformationForm.ValidateAll())
	    {
		    CreatePerson();
	    }
	    
    }
}