﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Views;


namespace KamerConnect.View.MAUI.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly FileService _fileService;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private const string _bucketName = "profilepictures";
    private IServiceProvider _serviceProvider;
    private Person _person;
    public Person _selectedPerson;
    public Match _match;


    public ProfilePage(FileService fileService, AuthenticationService authenticationService,
        PersonService personService, IServiceProvider serviceProvider, MatchService matchService)
    {
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();
        _fileService = fileService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _personService = personService;
        GetCurrentPerson().GetAwaiter().GetResult();
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is Match match)
        {
            _match = match;
            _selectedPerson = _personService.GetPersonById(match.personId);
            
            ProfileImage.Source = !string.IsNullOrEmpty(_selectedPerson.ProfilePicturePath)
                ? _fileService.GetFilePath(_bucketName, _selectedPerson.ProfilePicturePath)
                : "geenProfiel.png";
            
            NameLabel.Text = $"{_selectedPerson.FirstName} {_selectedPerson.MiddleName} {_selectedPerson.Surname}".Trim();
            PhoneLabel.Text = _selectedPerson.PhoneNumber ?? "Geen telefoonnummer beschikbaar";
            EmailLabel.Text = _selectedPerson.Email;
            GenderLabel.Text = _selectedPerson.Gender.GetDisplayName();
            BirthLabel.Text =  _selectedPerson.BirthDate.ToShortDateString();
            SchoolLabel.Text = _selectedPerson.Personality?.School ?? "Geen school opgegeven";
            CourseLabel.Text = _selectedPerson.Personality?.Study ?? "Geen studie opgegeven";
            DescriptionLabel.Text = _selectedPerson.Personality?.Description ?? "Geen beschrijving beschikbaar";
            MotivationLabel.Text = "ik vind het gewelig huis waar ik graag zou willen wonen: ...";
             

        }
    }
    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }

    private void AcceptButton_OnClicked(object? sender, EventArgs e)
    {
        
        _matchService.UpdateMatch(_match, status.Accepted);
        AcceptButton.IsVisible = false;
        RejectButton.IsVisible = false;
        AcceptLabel.IsVisible = true;
    }

    private void RejectButton_OnClicked(object? sender, EventArgs e)
    {
        _matchService.UpdateMatch(_match, status.Rejected);
        AcceptButton.IsVisible = false;
        RejectButton.IsVisible = false;
        RejectLabel.IsVisible = true;
    }
}