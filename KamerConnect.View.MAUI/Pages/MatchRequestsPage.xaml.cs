using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Pages;

public partial class MatchRequestsPage : ContentPage
{
    private readonly FileService _fileService;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private const string _bucketName = "profilepictures";
    private IServiceProvider _serviceProvider;
    private readonly HouseService _houseService;
    private Person _person;
    public MatchRequestsPage(HouseService houseService, FileService fileService, AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider, MatchService matchService)
    {
        InitializeComponent();
        _houseService = houseService;
        _serviceProvider = serviceProvider;
        _fileService = fileService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _personService = personService;
        MatchView.Content = _serviceProvider.GetRequiredService<MatchRequestsView>();
    }
    
}