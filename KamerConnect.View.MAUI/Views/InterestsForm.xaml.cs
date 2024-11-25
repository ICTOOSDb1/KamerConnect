using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class InterestsForm : ContentView
{
	private readonly PersonService _personService;
	private Person? _currentPerson;
	
	public InterestsForm(PersonService personService, Person person)
	{
		_personService = personService;
		_currentPerson = person;
		CheckIfPersonalityFieldsAreNull(person);
		BindingContext = person.Personality;
		InitializeComponent();
	}

	private void Button_update_interests(object? sender, EventArgs e)
	{
		ValidateIfAccountDetailsChanged(_currentPerson);
		_personService.UpdatePersonality(_currentPerson);
		CheckIfSocialsArePicked(_currentPerson);
		_personService.UpdateSocial(_currentPerson);
	}
	
	public void ValidateIfAccountDetailsChanged(Person person)
    {

        if (person.Personality.School != schoolEntry.DefaultText)
        {
	        person.Personality.School = schoolEntry.DefaultText;
        }
        if (person.Personality.Study != studyEntry.DefaultText)
        {
	        person.Personality.Study = studyEntry.DefaultText;
        }
        if (person.Personality.Description != descriptionEntry.DefaultText)
        {
	        person.Personality.Description = descriptionEntry.DefaultText;
        }
        if (person.Social.Url != socialUrlEntry.DefaultText)
        {
	        person.Social.Url = socialUrlEntry.DefaultText;
        }
    }
	
	private void CheckIfPersonalityFieldsAreNull(Person person)
	{
		if (person.Personality == null)
		{
			person.Personality = new Personality(null, null, null);
		}
		if (person.Personality.Description == null)
		{
			person.Personality.Description = "";
		}
		if (person.Personality.School == null)
		{
			person.Personality.School = "";
		}
		if (person.Personality.Study == null)
		{
			person.Personality.Study = "";
		}
		if (person.Social == null)
		{
			person.Social = new Social(SocialType.Facebook, "");
		}
		if (person.Social.Url == null)
		{
			person.Social.Url = "";
		}
	}
	
	public void CheckIfSocialsArePicked(Person person)
	{
		var selectedOption = SocialtypePicker.SelectedItem?.ToString();
		if (Enum.TryParse(selectedOption, true, out SocialType socialType))
		{
			if (person.Social == null)
			{
				person.Social = new Social(socialType, "");
			}
			else
			{
				person.Social.Type = socialType;
			}
		}
	}

}