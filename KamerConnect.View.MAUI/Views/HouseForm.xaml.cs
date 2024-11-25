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
                apartmentEntry.IsChecked = true;
                break;

            case HouseType.Studio:
                apartmentEntry.IsChecked = true;
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
                    string fileName = Path.GetFileName(filePath);
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

    private void OnPublish(object sender, EventArgs e)
    {
        string street = streetEntry.DefaultText;
        int houseNumber = int.Parse(houseNumberEntry.DefaultText);
        string addition = additionEntry.DefaultText;
        string city = cityEntry.DefaultText;
        string postalCode = postalCodeEntry.DefaultText;
        double price = double.Parse(priceEntry.DefaultText);
        int surface = int.Parse(surfaceEntry.DefaultText);
        int residents = int.Parse(residentsEntry.DefaultText);
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
            _houseService.Create(new House(
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
    }

}