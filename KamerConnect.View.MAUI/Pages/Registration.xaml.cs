using System.ComponentModel;
using System.Runtime.CompilerServices;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Pages;

public partial class Registration : ContentPage, INotifyPropertyChanged
{
	private readonly IServiceProvider _serviceProvider;
	private readonly AuthenticationService _authenticationService;

	private Role _selectedTab;
	public Role SelectedTab
	{
		get => _selectedTab;
		set
		{
			_selectedTab = value;
			OnPropertyChanged();
		}
	}

	public Registration(IServiceProvider serviceProvider, AuthenticationService authenticationService)
	{
		_serviceProvider = serviceProvider;
		_authenticationService = authenticationService;
		SelectTabAction = SelectTab;
		SelectedTab = Role.Seeking;

		NavigationPage.SetHasNavigationBar(this, false);

		InitializeComponent();

		UpdateButtonColors();
		BindingContext = this;
	}
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public Action<Role> SelectTabAction { get; set; }

	private async void Back(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Count > 1)
		{
			await Navigation.PopAsync();
		}
	}

	private void OnSearchingClicked(object sender, EventArgs e)
	{
		SearchingButton.TextColor = Colors.White;
		HavingButton.TextColor = Colors.Black;
		SelectTab(Role.Seeking);
		Submit.Text = "Verder";
	}

	private void OnHavingClicked(object sender, EventArgs e)
	{
		HavingButton.TextColor = Colors.White;
		SearchingButton.TextColor = Colors.Black;
		SelectTab(Role.Offering);

		Submit.Text = "Registreren";
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

	private void SelectTab(Role role)
	{
		SelectedTab = role;
		UpdateButtonColors();
	}

	private void UpdateButtonColors()
	{
		if (SelectedTab == Role.Seeking)
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

	private Person CreatePerson()
	{
		return new Person(
			personalInformationForm.Email,
			personalInformationForm.FirstName,
			personalInformationForm.MiddleName,
			personalInformationForm.Surname,
			personalInformationForm.PhoneNumber,
			personalInformationForm.BirthDate.Value,
			personalInformationForm.Gender,
			SelectedTab,
			null,
			Guid.NewGuid()
		);
	}

	private async void OnSubmit(object? sender, EventArgs e)
	{
		if (personalInformationForm.ValidateAll())
		{
			var person = CreatePerson();
			if (Application.Current.MainPage is NavigationPage navigationPage)
			{
				if (SelectedTab == Role.Seeking)
				{
					await navigationPage.Navigation.PushAsync(new RegisterHomePreferencesPage(_serviceProvider, person, personalInformationForm.Password));
				}
				else
				{
					_authenticationService.Register(person, personalInformationForm.Password);
					await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<LoginPage>());
				}
			}

		}
	}

}