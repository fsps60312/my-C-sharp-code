using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MegapolisClient
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            var grid = new Grid
            {
                RowDefinitions =
                    {
                        new RowDefinition{Height= new GridLength(2,GridUnitType.Star) },
                        new RowDefinition{Height= new GridLength(1,GridUnitType.Star) }
                    },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            grid.Children.Add(new Editor
            {
                Text = "Editor",
                VerticalOptions = LayoutOptions.FillAndExpand
            }, 0, 0);
            grid.Children.Add(new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content=new StackLayout
                {
                    VerticalOptions=LayoutOptions.FillAndExpand,
                    Children=
                    {
                        new Button
                        {
                            Text="Click Start",
                            ClassId="ClickStart"
                        }
                    }
                }
            }, 0, 1);
            var content = new ContentPage
            {
                Title = "MegapolisClient",
                Content = grid
            };

            MainPage = new NavigationPage(content);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
