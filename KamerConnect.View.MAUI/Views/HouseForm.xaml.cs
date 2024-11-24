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
    public ObservableCollection<IPickFile> ImageResults { get; set; } = new ObservableCollection<IPickFile>();
    private List<HouseImage> houseImages = new List<HouseImage>();

    public HouseForm(FileService fileService, HouseService houseService)
    {
        _fileService = fileService;
        _houseService = houseService;
        InitializeComponent();
        BindingContext = this;
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
                    var fileBytes = await File.ReadAllBytesAsync(filePath);
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
}