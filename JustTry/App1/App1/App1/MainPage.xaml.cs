using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using System.Diagnostics;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        private Grid GDmain,GDslide;
        private Button btn;
        private StackLayout sl;
        //public StackLayout slideMenu;
        public void Slide()
        {
            if (GDmain.ColumnDefinitions[0].Width.GridUnitType == GridUnitType.Auto)
            {
                GDmain.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Star);
                sl.IsVisible = false;
            }
            else
            {
                GDmain.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
                sl.IsVisible = true;
            }
        }
        public MainPage()
        {
            InitializeComponent();
            {
                GDmain = new Grid();
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Star) });
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                {
                    RelativeLayout rl = new RelativeLayout();
                    {
                        StackLayout sl = new StackLayout();
                        sl.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                        for (int i = 0; i < 10; i++)
                        {
                            Label l = new Label();
                            l.FontFamily = "Consolas";
                            l.FontSize = 10;
                            l.Text = "1";
                            l.VerticalOptions = LayoutOptions.Start;
                            sl.Children.Add(l);
                        }
                        {
                            btn = new Button();
                            btn.FontFamily = "Consolas";
                            btn.FontSize = 10;
                            btn.Text = "2";
                            btn.VerticalOptions = LayoutOptions.Start;
                            btn.Clicked += Btn_Clicked;
                            sl.Children.Add(btn);
                        }
                        //g.Children.Add(LayoutView, 1, 0);
                        rl.Children.Add(sl, Constraint.Constant(0.0), Constraint.Constant(0.0), Constraint.RelativeToParent((parent) => { return parent.Width; }), Constraint.RelativeToParent((parent) => { return parent.Height; }));
                    }
                    {
                        GDslide = new Grid();
                        GDslide.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                        GDslide.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        {
                            sl = new StackLayout();
                            sl.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                            sl.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
                            for (int i = 0; i < 10; i++)
                            {
                                Label l = new Label();
                                l.FontFamily = "Consolas";
                                l.FontSize = 10;
                                l.TextColor = Color.FromRgb(0.5, 0.5, 0.5);
                                l.Text = $"label{i}";
                                l.VerticalOptions = LayoutOptions.Start;
                                sl.Children.Add(l);
                            }
                            GDslide.Children.Add(sl, 0, 0);
                        }
                        rl.Children.Add(GDslide
                            , Constraint.Constant(0.0)
                            , Constraint.Constant(0.0)
                            , Constraint.RelativeToParent((parent) => { return parent.Width; })
                            , Constraint.RelativeToParent((parent) => { return parent.Height; }));
                        //rl.Children[0]
                    }
                    GDmain.Children.Add(rl, 1, 0);
                }
                this.Content = GDmain;
            }
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            Slide();
            (sender as Button).Text = "Running";
            //var cancelToken = new CancellationTokenSource();
            (sender as Button).Animate("hi", new Animation(new Action<double>((double v) =>
            {
                var l = btn;
                v %= 2.0;
                Debug.Assert(0.0 <= v && v < 2.0, $"v={v}");
                if (v < 1.0) l.FontSize = 10 * v + 30 * (1.0 - v);
                else l.FontSize = 30 * (v - 1.0) + 10 * (2.0 - v);
            }), 0.0, 10.0, null, new Action(() => { (sender as Button).Text = "Stopped"; })), 10, 10000);
            //Device.BeginInvokeOnMainThread(new Action(() =>
            //{
            //    MainPage.DisplayAlert("hi", "hello", "Yes", "No");
            //}));
            //cancelToken.CancelAfter(new TimeSpan(0, 0, 5));
            //Task.Factory.StartNew(() =>
            //{
            //    bool inc = true;
            //    while (true)
            //    {
            //        Task.Delay(1000);
            //        if (l.FontSize >= 30.0) inc = false;
            //        if (l.FontSize <= 10.0) inc = true;
            //        Device.BeginInvokeOnMainThread(new Action(() =>
            //        {
            //            //MainPage.DisplayAlert("", "1", "OK");
            //            if (l.FontSize==10) l.FontSize = 30.0;
            //            else l.FontSize = 10.0;
            //        }));
            //    }
            //}, cancelToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            //(sender as Button).Text = "Stopped";
        }
    }
}
