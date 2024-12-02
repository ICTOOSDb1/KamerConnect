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

    public HouseType Type => HouseType.Apartment;

    public HouseForm(FileService fileService, HouseService houseService,
        Person person)
    {
        _fileService = fileService;
        _houseService = houseService;
        _person = person;
        houseImages = new ObservableCollection<HouseImage>();

        GetCurrentHouse();
        InitializeComponent();
        BindingContext = this;
    }

    private void GetCurrentHouse()
    {
        House = _houseService.GetByPersonId(_person.Id);

        House.HouseImages.ForEach(houseImages.Add);
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

        HouseType houseType = Type;

        if (House == null)
        {
            _houseService.Create(new House(
                    Guid.NewGuid(),
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
                    houseImages.ToList()
                ),
                _person.Id
            );
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
                houseImages.ToList()
            ));
        }

        await Application.Current?.MainPage?.DisplayAlert("Huis opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

}