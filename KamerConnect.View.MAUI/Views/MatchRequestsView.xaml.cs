using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using Microsoft.Maui.Controls.Shapes;
using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;

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
        AddLegend();
        GetMatchRequests();
        
        DisplayStatus(2, Role.Seeking, MatchRequestStatus.Accepted);
        DisplayStatus(4, Role.Offering, MatchRequestStatus.Rejected);
        DisplayStatus(3, Role.Offering, MatchRequestStatus.Accepted);
        DisplayStatus(1, Role.Seeking, MatchRequestStatus.Pending);
        
        /*AddAcceptAndRejectButtons(15, Role.Offering);*/
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
            var separator = new BoxView
            {
                HeightRequest = 2,
                BackgroundColor = Colors.LightGray,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End
            };
            var label1 = new Label {Text = $"Label {i} 2", FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label {Text = $"Label {i} 3", FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label {Text = $"Label {i} 4", FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(separator, 0, i);
            Grid.SetColumnSpan(separator, 6);
            MatchRequests.Add(border, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
        }
    }

    public void AddLegend()
    {
        MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
        var columns = new List<Label>
        {
            new Label {Text = "Straat", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Stad", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Type", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Prijs", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center},
            new Label {Text = "Match request", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center}
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
        var separator = new BoxView
        {
            HeightRequest = 2,
            BackgroundColor = Colors.LightGray,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        MatchRequests.Add(separator, 0, 0);
        Grid.SetColumnSpan(separator, 6);
    }

    public void DisplayStatus(int row, Role role, MatchRequestStatus status)
    {
        if (role == Role.Seeking)
        {
            var statusLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            var statusImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Scale = 0.2,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.Transparent,
            };
            switch (status)
            {
                case MatchRequestStatus.Accepted:
                    statusLabel.Text = "Geaccepteerd";
                    statusImage.Source = "circleaccepted.png";
                    break;
                case MatchRequestStatus.Pending:
                    statusLabel.Text = "In behandeling";
                    statusImage.Source = "circlepending.png";
                    break;
                case MatchRequestStatus.Rejected:
                    statusLabel.Text = "Geweigerd";
                    statusImage.Source = "circlerejected.png";
                    break;
            }
            var buttonContainer = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 5,
            };
            MatchRequests.Add(statusImage, 5, row);
            MatchRequests.Add(statusLabel, 5, row);
        }
    }

    public void AddAcceptAndRejectButtons(int rows, Role role)
    {
        if (role == Role.Offering)
        {
            for (int i = 1; i < rows; i++)
            {
                var buttonContainer = new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                var acceptButton = new Image
                {
                    Source = "accept.png",
                    Scale = 0.3,
                    BackgroundColor = Colors.Transparent,
                    AutomationId = $"AcceptButton{i}"
                };
                var rejectButton = new Image
                {
                    Source = "reject.png",
                    Scale = 0.3,
                    BackgroundColor = Colors.Transparent,
                    AutomationId = $"RejectButton{i}"
                };
                buttonContainer.Children.Add(acceptButton);
                buttonContainer.Children.Add(rejectButton);
                MatchRequests.Add(buttonContainer, 5, i);
            }
        }
    }
}