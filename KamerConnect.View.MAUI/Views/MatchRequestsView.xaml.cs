using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{
    
    public MatchRequestsView()
    {
        _currentMatchService = matchService;
        InitializeComponent();
        GetMatchRequests();
        AddLegend();
        PlaceMatchRequests();
    }

    public void PlaceMatchRequests()
    {
        for (int i = 1; i < 6; i++)
        {
            var border = new Border
            {
                WidthRequest = 100,
                HeightRequest = 100,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(10)
                },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(-100)
            };

            var label1 = new Label { Text = $"Label {i} 2", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label { Text = $"Label {i} 3", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label { Text = $"Label {i} 4", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(border, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
        }
    }

    public void AddLegend()
    {
        /*var image = new Image
        {   Scale = 0.3,
            Source = "arrowupdown.png",
            Aspect = Aspect.Center,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };*/
        
        var columns = new List<Label>
        {
            new Label { Text = "Straat", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Stad", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Type", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Prijs", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Match request", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };
        
        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);
            MatchRequests.Add(new Image
            {
                Scale = 0.3,
                Source = "arrowupdown.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            }, i + 1, 0);
        }
    }

}