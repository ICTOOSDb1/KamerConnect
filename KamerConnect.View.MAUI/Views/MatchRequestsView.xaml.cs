using System;
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
            var label = new Label { Text = $"Label {i} 1" };
            var label1 = new Label { Text = $"Label {i} 2"};
            var label2 = new Label { Text = $"Label {i} 3"};
            var label3 = new Label { Text = $"Label {i} 4"};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(label, i, 0);
            MatchRequests.Add(label1, i, 1);
            MatchRequests.Add(label2, i, 2);
            MatchRequests.Add(label3, i, 3);
        }
    }
}