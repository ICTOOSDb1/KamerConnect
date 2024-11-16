using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.reusableItems;

public partial class MultiSelectComboBox : ContentView
{
    public ObservableCollection<SelectableItem> Items { get; set; } = new ObservableCollection<SelectableItem>();

    public string SelectedItemsDisplay => string.Join(", ", Items.Where(i => i.IsSelected).Select(i => i.Name));

    public MultiSelectComboBox()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Optional: Update selection on change
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SelectedItemsDisplay));
    }
}

public class SelectableItem
{
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}