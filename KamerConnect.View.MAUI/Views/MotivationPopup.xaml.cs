using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Views;

public partial class MotivationPopup : ContentView
{
    private TaskCompletionSource<string?> _taskCompletionSource;

    public MotivationPopup()
    {
        InitializeComponent();
        IsVisible = false;
    }

    public Task<string?> DisplayMotivationPopup()
    {
        _taskCompletionSource = new TaskCompletionSource<string?>();

        IsVisible = true;
        return _taskCompletionSource.Task;
    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        MotivationInput.Text = string.Empty;
        _taskCompletionSource.SetResult(null);
        IsVisible = false;
    }

    private void OnSendMotivationButtonClicked(object sender, EventArgs e)
    {
        _taskCompletionSource.SetResult(MotivationInput.Text);
        IsVisible = false;
    }
}