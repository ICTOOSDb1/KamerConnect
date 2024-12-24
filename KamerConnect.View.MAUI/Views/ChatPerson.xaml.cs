using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.Views;

public partial class ChatPerson : ContentView
{
    private const string _bucketName = "profilepictures";
    public event EventHandler ChatIsClicked;
    public ChatPerson()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }
    
    public string GetFilePath(string bucketName, string fileName)
    {
        var endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT");

        if (string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Minio environment variables are missing. Please check your .env file.");
        }

        return $"http://{endpoint}/{bucketName}/{fileName}";
    }
    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is Chat chat)
        {
            if (chat.PersonsInChat.Count > 0)
            {
                ProfileImage.Source = !string.IsNullOrEmpty(chat.PersonsInChat[0].ProfilePicturePath)
                    ? GetFilePath(_bucketName, chat.PersonsInChat[0].ProfilePicturePath)
                    : "user.png";
            }
            else
            {
                ProfileImage.Source = null;
            }
        }
    }
}