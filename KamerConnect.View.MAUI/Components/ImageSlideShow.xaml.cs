using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Components;

public partial class ImageSlideShow : ContentView
{
    
    public List<string> Images { get; set; } = new()
    {
        "dotnet_bot.png",
        "logo.png",
        "login_image.png"
    };
    
    private int _currentImageIndex = 0;
    
    public int CurrentImageIndex
    {
        get => _currentImageIndex;
        set
        {
            if (_currentImageIndex != value)
            {
                _currentImageIndex = value;
                SetCurrentImage(value);
            }
        }
    }
    
    public ImageSlideShow()
    {
        InitializeComponent();

        AddImagesToScrollView(Images);
        
        SetCurrentImage(_currentImageIndex);
    }

    private void SetCurrentImage(int index)
    {
        MainImage.Source = Images[index];
    }

    private void Next(object? sender, EventArgs eventArgs)
    {
        if (CurrentImageIndex < Images.Count - 1)
        {
            CurrentImageIndex++;
        }
        else
        {
            CurrentImageIndex = 0;
        }
        
    }

    private void Previous(object? sender, EventArgs eventArgs)
    {
        if (CurrentImageIndex > 0)
        {
            CurrentImageIndex--;
        }
        else
        {
            CurrentImageIndex = Images.Count - 1;
        }
    }

    private void SelectImage(object? sender, EventArgs eventArgs)
    {
        if (sender is ImageButton imageButton && imageButton.CommandParameter is int index)
        {
            CurrentImageIndex = (int)imageButton.CommandParameter;
            var grayOverlay = imageButton.BindingContext as BoxView;
            
            if (grayOverlay != null)
            {
                grayOverlay.IsEnabled = true;
            }
        }
    }

    
    private void AddImagesToScrollView(List<string> imageSources)
    {
        int index = 0;
        foreach (var imageSource in imageSources)
        {
            var imageGrid = new Grid();
            
            var imageButton = new ImageButton()
            {
                Source = imageSource,
                Aspect = Aspect.AspectFill,
                CommandParameter = index,
                HeightRequest = 150,
                WidthRequest = 150
            };
            imageButton.Clicked += SelectImage;
            
            imageGrid.Children.Add(imageButton);
            
            var grayOverlay = new BoxView
            {
                BackgroundColor = Colors.Gray,
                Opacity = 0
            };
            
            imageGrid.Children.Add(grayOverlay);
            
            grayOverlay.IsEnabled = false;
            
            PreviewImages.Children.Add(imageGrid);
            index++;
        }
    }
}