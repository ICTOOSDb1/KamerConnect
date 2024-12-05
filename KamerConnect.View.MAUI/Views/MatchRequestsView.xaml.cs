using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using Microsoft.Maui.Controls.Shapes;
using CommunityToolkit.Maui;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{

    public enum MatchRequestStatus
    {
        Accepted,
        Pending,
        Rejected
    }
    public MatchRequestsView()
    {
        InitializeComponent();
        GetMatchRequests();
        AddLegend();
        var statusImage = new Image
        {
            Scale = 0.3,
            Source="accepted2.png",
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Transparent,
        };
        MatchRequests.Add(statusImage, 3, 2);
    }

    public void GetMatchRequests()
    {
        for (int i = 1; i < 15; i++)
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
                Content = new Image
                {
                    Source = "logo.png",
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };
            var label1 = new Label {Text = $"Label {i} 2", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label {Text = $"Label {i} 3", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label {Text = $"Label {i} 4", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(border, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
        }
    }

    public void AddLegend()
    {
        var columns = new List<Label>
        {
            new Label {Text = "Straat", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Stad", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Type", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Prijs", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label {Text = "Match request", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center}
        };

        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);
            /*MatchRequests.Add(new Image
            {
                Scale = 0.3,
                Source = "arrowupdown.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            }, i + 1, 0);*/
        }
    }

    public void DisplayStatus(int row, Role role, MatchRequestStatus status)
    {
        var statusLabel = new Label();
        var statusImage = new Image
        {
            Scale = 0.3,
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Transparent,
        };
        if (role == Role.Offering)
        {
            switch (status)
            {
                case MatchRequestStatus.Accepted:
                    statusLabel.Text = "Accepteren";
                    statusImage.Source = "accept.png";
                    break;
                case MatchRequestStatus.Rejected:
                    statusLabel.Text = "Weigeren";
                    statusImage.Source = "reject.png";
                    break;
            }
        }
        if (role == Role.Seeking)
        {
            switch (status)
            {
                case MatchRequestStatus.Accepted:
                    statusLabel.Text = "Geaccepteerd";
                    statusImage.Source = "circleAccepted.png";
                    break;
                case MatchRequestStatus.Pending:
                    statusLabel.Text = "In behandeling";
                    statusImage.Source = "circlePending.png";
                    break;
                case MatchRequestStatus.Rejected:
                    statusLabel.Text = "Geweigerd";
                    statusImage.Source = "circleRejected.png";
                    break;
            }
        }
        MatchRequests.Add(statusImage, 6, row + 1);
        MatchRequests.Add(statusImage, 6, row + 1);
    }
}