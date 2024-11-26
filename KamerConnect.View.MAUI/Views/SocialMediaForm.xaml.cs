using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class SocialMediaForm : ContentView
{
    private readonly PersonService _personService;
    private Person? _currentPerson;

    public SocialMediaForm(PersonService personService, Person person)
    {
        _personService = personService;
        _currentPerson = person;
        if (person.Social == null)
        {
            person.Social = new Social(null, null);
        }
        BindingContext = person.Social;
        InitializeComponent();
    }

    private void Button_Update_social_media(object sender, EventArgs e)
    {
        CheckIfSocialsArePicked(_currentPerson);
        _personService.UpdateSocial(Guid.Parse(_currentPerson.Id), _currentPerson.Social);
    }
    
    public void CheckIfSocialsArePicked(Person person)
    {
        var selectedOption = SocialtypePicker.SelectedItem?.ToString();
        if (Enum.TryParse(selectedOption, true, out SocialType socialType))
        {
                person.Social.Type = socialType;
        }
    }
}
