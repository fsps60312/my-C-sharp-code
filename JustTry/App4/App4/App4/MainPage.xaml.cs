using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
	public partial class MainPage : ContentPage
    {
        EntryCell A, B;
        public static readonly BindableProperty BindingTextProperty = BindableProperty.Create("BindingText", typeof(string), typeof(MainPage), "default value");
        public string BindingText
        {
            get
            {
                return (string)GetValue(BindingTextProperty);
            }
            set
            {
                SetValue(BindingTextProperty, value);
            }
        }
        public MainPage()
		{
            InitializeComponent();
            //s = "default value";
            A = new EntryCell
            {
                Label = "A",
                Keyboard = Keyboard.Default
            };
            B = new EntryCell
            {
                Label = "B",
                Keyboard = Keyboard.Default
            };

            A.SetBinding(EntryCell.TextProperty, "BindingText", BindingMode.TwoWay);
            A.BindingContext = this;
            B.SetBinding(EntryCell.TextProperty, "BindingText", BindingMode.TwoWay);
            B.BindingContext = this;

            Content = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Table Title") {
                    new TableSection ("TableSection Title") { A, B }
                }
            };
		}
	}
}
