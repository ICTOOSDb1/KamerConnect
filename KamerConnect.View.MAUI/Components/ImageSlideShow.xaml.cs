using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Components;

public partial class ImageSlideShow : ContentView
{
    
    //public List<string> Images { get; set; }
    
    public static readonly BindableProperty ImagesProperty = BindableProperty.Create(
        nameof(Images),
        typeof(List<HouseImage>),
        typeof(ImageSlideShow),
        null, BindingMode.TwoWay);

    public List<HouseImage> Images
    {
        get => (List<HouseImage>)GetValue(ImagesProperty);
        set => SetValue(ImagesProperty, value);
    }
    
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
    
    public ImageSlideShow(List<HouseImage> images)
    {
        Images = images;
        
        InitializeComponent();

        AddImagesToScrollView(Images);
        
        SetCurrentImage(_currentImageIndex);
    }

    private void SetCurrentImage(int index)
    {
        MainImage.Source = Images[index].FullPath;
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
                grayOverlay.Opacity = 0.5;
            }
        }
    }

    
    private void AddImagesToScrollView(List<HouseImage> imageSources)
    {
        int index = 0;
        foreach (var imageSource in imageSources)
        {
            var imageGrid = new Grid();
            
            var imageFrame = new Frame
            {
                CornerRadius = 15,
                Margin = 5,
                HasShadow = false,
                HeightRequest = 150,
                WidthRequest = 150
            };

            var imageButton = new ImageButton()
            {
                Source = imageSource.FullPath,
                Aspect = Aspect.AspectFill,
                CommandParameter = index,
                HeightRequest = 150,
                WidthRequest = 150,
            };

            imageButton.Clicked += SelectImage;

            // Add the ImageButton to the Frame
            imageFrame.Content = imageButton;

            imageGrid.Children.Add(imageFrame);

            var grayOverlay = new BoxView
            {
                BackgroundColor = Colors.Gray,
                Opacity = 0
            };

            imageGrid.Children.Add(grayOverlay);

            PreviewImages.Children.Add(imageGrid);
            index++;
        }
    }

}