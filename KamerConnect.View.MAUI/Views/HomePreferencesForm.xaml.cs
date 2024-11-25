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
		CheckIfHousePreferencesFieldsAreNull(person);
		BindingContext = _currentPerson.HousePreferences;
		InitializeComponent();
	}

	private void Button_Update_house_preferences(object sender, EventArgs e)
	{
		ValidateIfAccountDetailsChanged(_currentPerson);
		CheckIfHomeTypeIsPicked(_currentPerson);
	}

	public void ValidateIfAccountDetailsChanged(Person person)
	{ 
		if (_currentPerson.HousePreferences.SurfaceArea != surfaceEntry.DefaultText)
		{
			_currentPerson.HousePreferences.SurfaceArea = surfaceEntry.DefaultText;
		}
		if (_currentPerson.HousePreferences.Budget != priceEntry.DefaultText)
		{
			_currentPerson.HousePreferences.Budget = priceEntry.DefaultText; 
		}
	}
	
    private void CheckIfHousePreferencesFieldsAreNull(Person person)
    {
	    if (_currentPerson.HousePreferences == null)
	    {
		    _currentPerson.HousePreferences = new HousePreferences(null, null, null, null);
	    }
	    if (_currentPerson.HousePreferences.SurfaceArea == null)
	    {
		    _currentPerson.HousePreferences.SurfaceArea = "";
	    }
	    if (_currentPerson.HousePreferences.Budget == null)
	    {
		    _currentPerson.HousePreferences.Budget = "";
	    }
    }
    
    public void CheckIfHomeTypeIsPicked(Person person)
    {
	    var selectedOption = HometypePicker.SelectedItem?.ToString();
	    if (Enum.TryParse(selectedOption, true, out HouseType houseType))
	    {
		    if (person.HousePreferences == null)
		    {
			    person.HousePreferences = new HousePreferences(null, null, null, null);
		    }
		    else
		    {
			    person.HousePreferences.Type = houseType;
		    }
	    }
    }
}