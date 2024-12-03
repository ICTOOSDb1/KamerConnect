﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{
    
    public MatchRequestsView()
    {
        InitializeComponent();
        GetMatchRequests();
    }

    public void GetMatchRequests()
    {
        for (int i = 1; i < 6; i++)
        {
            var image = new Image
            {
                Scale = 0.3,
                Source = "logo.png",
                Aspect = Aspect.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(-100)
            };

            var label1 = new Label { Text = $"Label {i} 2", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label { Text = $"Label {i} 3", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label { Text = $"Label {i} 4", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(image, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
        }
    }
}