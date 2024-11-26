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
    public ObservableCollection<IPickFile> ImageResults { get; set; } = new ObservableCollection<IPickFile>();
    private List<HouseImage> houseImages = new List<HouseImage>();

    public HouseForm(FileService fileService, HouseService houseService,
        Person person)
    {
        _fileService = fileService;
        _houseService = houseService;
        _person = person;

        GetCurrentHouse();
        InitializeComponent();
        SetDefaultHouseType();
        BindingContext = this;
    }

    private void SetDefaultHouseType()
    {
        switch (House?.Type)
        {
            case HouseType.Apartment:
                apartmentEntry.IsChecked = true;
                break;

            case HouseType.House:
                houseEntry.IsChecked = true;
                break;

            case HouseType.Studio:
                studioEntry.IsChecked = true;
                break;
        }
    }

    private void GetCurrentHouse()
    {
        if (_person.HouseId != null)
        {
            House = _houseService.Get((Guid)_person.HouseId);
        }
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
                    ImageResults.Add(result);

                    string filePath = result.FileResult!.FullPath;
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(filePath);
                    string contentType = FileUtils.GetContentType(fileName);

                    houseImages.Add(new HouseImage(fileName, bucketName));

                    await _fileService.UploadFileAsync(bucketName, fileName, filePath, contentType);
                }
            }
    }

    private void OnRemoveFileClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var file = (IPickFile)button.CommandParameter;

        if (ImageResults.Contains(file))
        {
            ImageResults.Remove(file);
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

        bool isHouseTypeValid = apartmentEntry.IsChecked || houseEntry.IsChecked || studioEntry.IsChecked;
        if (!isHouseTypeValid)
        {
            radiobuttonNotSelected.IsVisible = true;
        }
        else
        {
            radiobuttonNotSelected.IsVisible = false;
        }

        return streetEntry.IsValid &&
               houseNumberEntry.IsValid &&
               additionEntry.IsValid &&
               cityEntry.IsValid &&
               postalCodeEntry.IsValid &&
               priceEntry.IsValid &&
               houseNumberEntry.IsValid &&
               surfaceEntry.IsValid &&
               residentsEntry.IsValid &&
               isHouseTypeValid;
    }

    private async void OnPublish(object sender, EventArgs e)
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

        HouseType houseType;
        if (apartmentEntry.IsChecked)
            houseType = HouseType.Apartment;
        else if (houseEntry.IsChecked)
            houseType = HouseType.House;
        else if (studioEntry.IsChecked)
            houseType = HouseType.Studio;
        else
        {
            houseType = HouseType.House;
        }

        if (House == null)
        {
            var houseId = _houseService.Create(new House(
                null,
                houseType,
                price,
                description,
                surface,
                residents,
                city,
                street,
                postalCode,
                houseNumber,
                addition,
                houseImages
            ));

            _person.HouseId = houseId;
        }
        else
        {
            _houseService.Update(new House(
                House.Id,
                houseType,
                price,
                description,
                surface,
                residents,
                city,
                street,
                postalCode,
                houseNumber,
                addition,
                houseImages
            ));
        }

        await Application.Current?.MainPage?.DisplayAlert("Huis opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

}