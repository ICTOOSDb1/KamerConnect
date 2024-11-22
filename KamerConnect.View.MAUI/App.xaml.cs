using Microsoft.Extensions.DependencyInjection;
using System;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = serviceProvider.GetRequiredService<UpdateAccount>(); //om updateaccount te testen

    }
}
