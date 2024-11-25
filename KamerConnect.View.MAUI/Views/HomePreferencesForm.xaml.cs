using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class HomePreferencesForm : ContentView
{
	private readonly PersonService _personService;
	private Person? _currentPerson;

	public HomePreferencesForm(PersonService personService, Person person)
	{
		_personService = personService;
		_currentPerson = person;
		if (person.HousePreferences == null)
		{
			person.HousePreferences = new HousePreferences(null, null, null, null);
		}
		BindingContext = _currentPerson.HousePreferences;
		InitializeComponent();
	}

	private void Button_Update_house_preferences(object sender, EventArgs e)
	{
		CheckIfHomeTypeIsPicked(_currentPerson);
		_personService.UpdateHousePreferences(_currentPerson);
	}
	
    
    public void CheckIfHomeTypeIsPicked(Person person)
    {
	    var selectedOption = HometypePicker.SelectedItem?.ToString();
	    if (Enum.TryParse(selectedOption, true, out HouseType houseType))
	    {
			    person.HousePreferences.Type = houseType;
		    
	    }
    }
}