using System.Collections.ObjectModel;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Utils;
using LukeMauiFilePicker;
namespace KamerConnect.View.MAUI.Views;

public partial class HouseForm : ContentView
{
    private const string bucketName = "house";
    private readonly FileService _fileService;
    private readonly HouseService _houseService;
    private readonly GeoLocationService _geoLocationService;
    private readonly Person _person;
    private House? _house;
    public House? House
    {
        get => _house;
        set
        {
            _house = value;
            OnPropertyChanged(nameof(House));
        }
    }
    private ObservableCollection<HouseImage> _houseImages;
    public ObservableCollection<HouseImage> houseImages
    {
        get => _houseImages;
        set
        {
            _houseImages = value;
            OnPropertyChanged();
        }
    }

    public HouseType Type;

    public HouseForm(FileService fileService, HouseService houseService,
        Person person, GeoLocationService geoLocationService)
    {
        _fileService = fileService;
        _houseService = houseService;
        _geoLocationService = geoLocationService;
        _person = person;

        houseImages = new ObservableCollection<HouseImage>();

        InitializeComponent();
        GetCurrentHouse();

        BindingContext = this;

        HouseTypePicker.SelectedItem = _house?.Type.GetDisplayName() ?? "Huis";
    }

    private void GetCurrentHouse()
    {
        House = _houseService.GetByPersonId(_person.Id);

        House?.HouseImages?.ForEach(houseImages.Add);

        SmokingTypePicker.SelectedItem = House?.Smoking.GetDisplayName() ?? PreferenceChoice.No_preference.GetDisplayName();
        PetTypePicker.SelectedItem = House?.Pet.GetDisplayName() ?? PreferenceChoice.No_preference.GetDisplayName();
        InteriorTypePicker.SelectedItem = House?.Interior.GetDisplayName() ?? PreferenceChoice.No_preference.GetDisplayName();
        ParkingTypePicker.SelectedItem = House?.Parking.GetDisplayName() ?? PreferenceChoice.No_preference.GetDisplayName();
    }

    private async void OnPickFilesClicked(object sender, EventArgs e)
    {
        IFilePickerService picker = new FilePickerService();

        var results = await picker.PickFilesAsync(
            "Selecteer foto's",
            MauiFileUtils.ImageFileTypes,
            true
        );

        if (results != null)
            foreach (var result in results)
            {
                if (result?.FileResult != null)
                {
                    string filePath = result.FileResult!.FullPath;
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(filePath);
                    string contentType = FileUtils.GetContentType(fileName);

                    await _fileService.UploadFileAsync(bucketName, fileName, filePath, contentType);
                    houseImages.Add(new HouseImage(fileName, bucketName));
                }
            }
    }

    private void OnRemoveFileClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var file = (HouseImage)button.CommandParameter;

        if (houseImages.Contains(file))
        {
            houseImages.Remove(file);
        }
    }

    public bool ValidateForm()
    {
        streetEntry.Validate();
        houseNumberEntry.Validate();
        cityEntry.Validate();
        postalCodeEntry.Validate();
        priceEntry.Validate();
        surfaceEntry.Validate();
        residentsEntry.Validate();

        return streetEntry.IsValid &&
                houseNumberEntry.IsValid &&
                additionEntry.IsValid &&
                cityEntry.IsValid &&
                postalCodeEntry.IsValid &&
                priceEntry.IsValid &&
                houseNumberEntry.IsValid &&
                surfaceEntry.IsValid &&
                residentsEntry.IsValid;
    }

    public async void OnPublish()
    {
        if (!ValidateForm()) return;

        string street = streetEntry.Text;
        int houseNumber = int.Parse(houseNumberEntry.Text);
        string addition = additionEntry.Text;
        string city = cityEntry.Text;
        string postalCode = postalCodeEntry.Text;
        double price = double.Parse(priceEntry.Text);
        int surface = int.Parse(surfaceEntry.Text);
        int residents = int.Parse(residentsEntry.Text);
        string description = descriptionEntry.Text;
        bool available = availableEntry.IsChecked;

        if (House == null)
        {
            House = new House(
                    Guid.NewGuid(),
                    Type,
                    price,
                    description,
                    surface,
                    residents,
                    city,
                    street,
                    postalCode,
                    houseNumber,
                    addition,
                    await _geoLocationService.GetGeoCode($"{street} {houseNumber}{addition} {city}"),
                    houseImages.ToList(),
                    available,
                    PreferenceChoiceTypeChanged(SmokingTypePicker),
                    PreferenceChoiceTypeChanged(PetTypePicker),
                    PreferenceChoiceTypeChanged(InteriorTypePicker),
                    PreferenceChoiceTypeChanged(ParkingTypePicker)
            );

            _houseService.Create(House,
                _person.Id
            );
        }
        else
        {
            _houseService.Update(new House(
                House.Id,
                Type,
                price,
                description,
                surface,
                residents,
                city,
                street,
                postalCode,
                houseNumber,
                addition,
                await _geoLocationService.GetGeoCode($"{street} {houseNumber}{addition} {city}"),
                houseImages.ToList(),
                available,
                PreferenceChoiceTypeChanged(SmokingTypePicker),
                PreferenceChoiceTypeChanged(PetTypePicker),
                PreferenceChoiceTypeChanged(InteriorTypePicker),
                PreferenceChoiceTypeChanged(ParkingTypePicker)
            ));
        }

        await Application.Current?.MainPage?.DisplayAlert("Huis opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

    private PreferenceChoice PreferenceChoiceTypeChanged(Picker picker)
    {
        switch ($"{picker.SelectedItem}")
        {
            case "Ja":
                return PreferenceChoice.Yes;
            case "Nee":
                return PreferenceChoice.No;
            case "Geen voorkeur":
                return PreferenceChoice.No_preference;
        }

        return PreferenceChoice.No_preference;
    }

    private async void HouseTypeChanged(object sender, EventArgs e)
    {
        switch ($"{HouseTypePicker.SelectedItem}")
        {
            case "Appartement":
                Type = HouseType.Apartment;
                break;
            case "Huis":
                Type = HouseType.House;
                break;
            case "Studio":
                Type = HouseType.Studio;
                break;
        }
    }

}