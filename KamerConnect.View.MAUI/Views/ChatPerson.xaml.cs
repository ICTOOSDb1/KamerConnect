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
    
    FileService _fileService;
    private const string _bucketName = "profilepictures";
    public ChatPerson(FileService fileService)
    {
        this._fileService = fileService;
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }
    
    /*private void OnChatTapped(object sender, EventArgs e)
    {
        if (BindingContext is Chat chat)
        {
            var chatView = MauiProgram.Services.GetRequiredService<HousePage>();
            chatView.BindingContext = chat;

            App.Current.MainPage = new NavigationPage(housePage);
        }
    }*/
    
    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is Chat chat)
        {
            if (chat.PersonsInChat.Count > 0)
            {
                ProfileImage.Source = !string.IsNullOrEmpty(chat.PersonsInChat[0].ProfilePicturePath)
                    ? _fileService.GetFilePath(_bucketName, chat.PersonsInChat[0].ProfilePicturePath)
                    : "geenProfiel.png";
            }
            else
            {
                ProfileImage.Source = null;
            }
        }
    }
}