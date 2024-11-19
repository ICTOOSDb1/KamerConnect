using Microsoft.Maui.Storage;
using Microsoft.UI.Xaml.Input;
using KamerConnect.View.MAUI.ViewModel;

namespace KamerConnect.View.MAUI.UpdateAccountInputs;

public partial class UpdateAccountsInput : ContentView
{
    public UpdateAccountsInput()
    {
        InitializeComponent();
        BindingContext = new UpdateAccountsInputViewModel();
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
