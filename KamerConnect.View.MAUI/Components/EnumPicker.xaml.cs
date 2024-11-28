using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Components;

public partial class EnumPicker : ContentView
    {
        public EnumPicker()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // Title for the label
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(EnumPicker), string.Empty);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(EnumPicker), -1, BindingMode.TwoWay);

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        // Enum Type Property
        public static readonly BindableProperty EnumTypeProperty =
            BindableProperty.Create(nameof(EnumType), typeof(Type), typeof(EnumPicker), null, propertyChanged: OnEnumTypeChanged);

        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        // Selected Enum Value
        public static readonly BindableProperty SelectedValueProperty =
            BindableProperty.Create(nameof(SelectedValue), typeof(object), typeof(EnumPicker), null, BindingMode.TwoWay);

        public object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }

        public List<string> EnumValues { get; private set; } = new();

        private static void OnEnumTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EnumPicker picker && newValue is Type enumType && enumType.IsEnum)
            {
                picker.EnumValues = Enum.GetNames(enumType).ToList();
                picker.OnPropertyChanged(nameof(EnumValues));
            }
        }

        private void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && EnumType != null)
            {
                var selectedIndex = picker.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < EnumValues.Count)
                {
                    SelectedValue = Enum.Parse(EnumType, EnumValues[selectedIndex]);
                }
            }
        }
}