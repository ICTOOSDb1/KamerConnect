using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KamerConnect.Models;
using KamerConnect.Services;
using System.ComponentModel;


namespace KamerConnect.View.MAUI;

public partial class PersonalInformationForm : ContentView, INotifyPropertyChanged
{

    public PersonalInformationForm()
    {
        InitializeComponent();
        BindingContext = new PersonalInformationForm();
    }
    private string _mail;
    public string Mail_
    {
        get => _mail;
        set
        {
            _mail = value;
            OnPropertyChanged(nameof(Mail_));
        }
    }

    private string _firstname;
    public string FirstName_
    {
        get => _firstname;
        set
        {
            _firstname = value;
            OnPropertyChanged(nameof(FirstName_));
        }
    }

    private string _prefix;
    public string Prefix_
    {
        get => _prefix;
        set
        {
            _prefix = value;
            OnPropertyChanged(nameof(Prefix_));
        }
    }

    private string _surname;
    public string Surname_
    {
        get => _surname;
        set
        {
            _surname = value;
            OnPropertyChanged(nameof(Surname_));
        }
    }

    private string _phonenumber;
    public string Phonenumber_
    {
        get => _phonenumber;
        set
        {
            _phonenumber = value;
            OnPropertyChanged(nameof(Phonenumber_));
        }
    }

    private DateTime _birthdate;
    public DateTime Birthdate_
    {
        get => _birthdate;
        set
        {
            _birthdate = value;
            OnPropertyChanged(nameof(Birthdate_));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public void CreatePersonModel()
    {



        //Person persoon = new Person(Email.Content, Firstname.Content, Prefix.Content, Surname.Content, Phonenumber.Content,
        //Birthdate.Content, Gender gender, Role role)

    }
}

   
