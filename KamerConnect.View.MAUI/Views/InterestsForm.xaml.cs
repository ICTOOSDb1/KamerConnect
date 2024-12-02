using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class InterestsForm : ContentView
{
	private readonly PersonService _personService;
	private Person _currentPerson;

	public InterestsForm(PersonService personService, Person person)
	{
		_personService = personService;
		_currentPerson = person;
		if (person.Personality == null)
		{
			person.Personality = new Personality(null, null, null);
		}
		BindingContext = person.Personality;
		InitializeComponent();
	}

	public async void Button_update_interests()
	{
		if (!ValidateForm()) return;

		_personService.UpdatePersonality(_currentPerson.Id, _currentPerson.Personality);
		await Application.Current?.MainPage?.DisplayAlert("Opgeslagen", "Succesvol opgeslagen!", "Ga verder");
	}

	public bool ValidateForm()
	{
		schoolEntry.Validate();
		studyEntry.Validate();

		return schoolEntry.IsValid &&
			   studyEntry.IsValid;
	}
}