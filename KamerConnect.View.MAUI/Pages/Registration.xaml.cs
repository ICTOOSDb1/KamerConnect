using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class Registration : ContentPage, INotifyPropertyChanged
{
	public enum Tab
	{
		SearchingHouse,
		HavingHouse
	}
	private Tab _selectedTab;
	public Tab SelectedTab
	{
		get => _selectedTab;
		set
		{
			_selectedTab = value;
			OnPropertyChanged();
		}
	}
	public Registration()
	{
		InitializeComponent();
		SelectTabAction = SelectTab;
		BindingContext = this;
		SelectedTab = Tab.SearchingHouse;
		UpdateButtonColors();
	}
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public Action<Tab> SelectTabAction { get; set; }

	private async void Terug(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Count > 1)
		{
			await Navigation.PopAsync();
		}
	}

	private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (sender is RadioButton radioButton && e.Value)
		{

		}
	}

	private void OnSearchingClicked(object sender, EventArgs e)
	{
		SelectTab(Tab.SearchingHouse);
	}

	private void OnHavingClicked(object sender, EventArgs e)
	{
		SelectTab(Tab.HavingHouse);
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
	
	private void SelectTab(Tab tab)
	{
		SelectedTab = tab;
		UpdateButtonColors();
	}

	private void UpdateButtonColors()
	{
		if (SelectedTab == Tab.SearchingHouse)
		{
			HuisButtonColor = "#EF626C";
			HuisgenootButtonColor = "#FFFFFF";
		}
		else
		{
			HuisButtonColor = "#FFFFFF";
			HuisgenootButtonColor = "#EF626C";
		}
	}

	private void CreatePerson()
	{
		Role role = SelectedTab == Tab.SearchingHouse ? Role.Seeking : Role.Offering;

		var newPerson = new Person(
			personalInformationForm.Email,
			personalInformationForm.FirstName,
			personalInformationForm.MiddleName,
			personalInformationForm.Surname,
			personalInformationForm.PhoneNumber,
			personalInformationForm.BirthDate.Value,
			Enum.Parse<Gender>(personalInformationForm.Gender ?? "Other"),
			role,
			null
		);
	}
	
	private async void submit(object? sender, EventArgs e)
	{
		if (personalInformationForm.ValidateAll())
		{
			CreatePerson();
			if (Navigation.NavigationStack.Count > 1)
			{
				await Navigation.PopAsync();
			}
			
		}
	}
}