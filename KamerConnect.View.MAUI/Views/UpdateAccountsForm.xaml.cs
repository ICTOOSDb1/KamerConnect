using Microsoft.Maui.Storage;
using Microsoft.UI.Xaml.Input;

namespace KamerConnect.View.MAUI.Views;

public partial class UpdateAccountsForm : ContentView
{
    public UpdateAccountsForm()
    {
        InitializeComponent();
    }

    private async void Image_tapped(object sender, TappedEventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            var filePath = result.FullPath;
            Console.WriteLine(filePath);
            profile_picture.Source = ImageSource.FromFile(filePath);
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}
