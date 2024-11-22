using Microsoft.Maui.Storage;


namespace KamerConnect.View.MAUI.UpdateAccountInputs;

public partial class UpdateAccountsInput : ContentView
{
    public UpdateAccountsInput()
    {
        InitializeComponent();
    }

    private async void Image_tapped(object sender, TappedEventArgs e)
    {
        // Open file picker and specify allowed file types (e.g., images)
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image",
            FileTypes = FilePickerFileType.Images // Allows selection of image files only
        });

        if (result != null)
        {
            // Use the selected file
            var filePath = result.FullPath;
            // You can load or display the image as needed, e.g., update the image source
            // Example: myImage.Source = ImageSource.FromFile(filePath);
            Console.WriteLine(filePath);
            profile_picture.Source = ImageSource.FromFile(filePath);
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}
