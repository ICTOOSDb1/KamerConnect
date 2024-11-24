using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class InterestsForm : ContentView
{
	private readonly PersonService _personService;
	private Person? _currentPerson;
	private Personality? personality;
	public Personality? Personality
	{
		get => personality;
		set
		{
			personality = value;
		}
	}
	
	public InterestsForm(PersonService personService, Person person)
	{
		_personService = personService;
		_currentPerson = person;
		Personality = person.Personality;
		CheckIfPersonalityFieldsAreNull(person);
		BindingContext = Personality;
		InitializeComponent();
	}

	private void Button_update_interests(object? sender, EventArgs e)
	{
		Guid personId = Guid.Parse(_currentPerson.Id);
		_personService.InsertTableIfIdIfNotExist(personId, "personality");
		ValidateIfAccountDetailsChanged(_currentPerson);
	}
	
	public void ValidateIfAccountDetailsChanged(Person person)
    {
        
        List<string> fieldsToUpdate = new List<string>();
        List<NpgsqlParameter> parametersToUpdate = new List<NpgsqlParameter>(); 
        
        if (Personality.School != schoolEntry.DefaultText)
        {
	        Personality.School = schoolEntry.DefaultText;
            fieldsToUpdate.Add("school = @School");
            parametersToUpdate.Add(new NpgsqlParameter("@School", Personality.School));
        }
        if (Personality.Study != studyEntry.DefaultText)
        {
	        Personality.Study = studyEntry.DefaultText;
	        fieldsToUpdate.Add("study = @Study");
	        parametersToUpdate.Add(new NpgsqlParameter("@Study", Personality.Study));
        }
        if (Personality.Description != descriptionEntry.DefaultText)
        {
	        Personality.Description = descriptionEntry.DefaultText;
	        fieldsToUpdate.Add("description = @Description");
	        parametersToUpdate.Add(new NpgsqlParameter("@Description", Personality.Description));
        }
        Guid personId = Guid.Parse(_currentPerson.Id);
        
        _personService.UpdatePerson(personId, "person_id", fieldsToUpdate, parametersToUpdate, "personality");
    }
	
	private void CheckIfPersonalityFieldsAreNull(Person person)
	{
		if (Personality == null)
		{
			Personality = new Personality(null, null, null);
		}
		if (Personality.Description == null)
		{
			Personality.Description = "";
		}
		if (Personality.School == null)
		{
			Personality.School = "";
		}
		if (Personality.Study == null)
		{
			Personality.Study = "";
		}
	}
}