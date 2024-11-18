using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.View.MAUI.ViewModels;

namespace KamerConnect.View.MAUI;

public partial class Register : ContentPage
{
    public Register()
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel();

    }
}