using KamerConnect.View.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.ViewModel
{
    public class UpdateAccountsInputViewModel : NotifyModel
    {
        public UpdateAccountsInputViewModel() {
            FirstName = "piet";
        }

        string? firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value != null)
                {
                    firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        public Command UpdateAccountCommand
        {
            get
            {
                return new Command((e) =>
                {
                    FirstName = "Puk";
                    Application.Current.MainPage.DisplayAlert("Account Updated", "Your account has been updated successfully.", "OK");
                });
            }
        }
    }
}
