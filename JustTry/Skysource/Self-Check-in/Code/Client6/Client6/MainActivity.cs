using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client6
{
    [Activity(Label = "Client6", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        void SetPage(Fragment page)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.Main, page);
            ft.Commit();
        }
        Dictionary<string,string>GetDoctorData(Dictionary<string,string>data)
        {
            Dictionary<string, string> answer=new Dictionary<string, string>();
            answer.Add("Doctor", "Burney Yu");
            answer.Add("Time slot", "18:00~18:30");
            return answer;
        }
        bool NotifyDoctors(Dictionary<string,string>data)
        {
            bool messageSentSuccessfully=false;
            bool decisionMade = false;
            new AlertDialog.Builder(this).SetTitle("Make your decision").SetMessage("Do doctors receive notification successfully?").SetPositiveButton("Succeed", (sender, args) =>
             {
                 messageSentSuccessfully = true;
                 decisionMade = true;
             }).SetNegativeButton("Failed", (sender, args) =>
             {
                 messageSentSuccessfully = false;
                 decisionMade = true;
             }).Show();
            while (!decisionMade) ;
            return messageSentSuccessfully;
        }
        //Fragment currentPage = null;
        LogoPage logoPage;
        FormPage formPage;
        protected override void OnCreate(Bundle bundle)
        {
            int initializingStage = 0;
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Toast.MakeText(this, string.Format("Initializing stage {0}...", ++initializingStage), ToastLength.Short).Show();
            logoPage = new LogoPage();
            logoPage.EventsEnabled += (sss, aaa) =>
            {
                Toast.MakeText(this, string.Format("Initializing stage {0}...", ++initializingStage), ToastLength.Short).Show();
                logoPage.logo.Click += delegate
                {
                    SetPage(formPage);
                };
                Toast.MakeText(this, string.Format("Initializing stage {0}...", ++initializingStage), ToastLength.Short).Show();
            };
            formPage = new FormPage();
            formPage.EventsEnabled += (sss, aaa) =>
              {
                  Toast.MakeText(this, string.Format("Initializing stage {0}...", ++initializingStage), ToastLength.Short).Show();
                  formPage.back.Click += delegate
                    {
                        new AlertDialog.Builder(this).SetTitle("You are going to quit!").SetMessage("Discard the form and go back to home page?").SetPositiveButton("Yes", (sender, args) =>
                         {
                             formPage.ClearTexts();
                             Toast.MakeText(this, "Back to home page...", ToastLength.Short).Show();
                             SetPage(logoPage);
                         }).SetNegativeButton("No", (sender, args) =>
                         {
                             Toast.MakeText(this, "Type in your check-in info", ToastLength.Long).Show();
                         }).Show();
                    };
                  formPage.ok.Click += delegate
                    {
                        formPage.Enabled = false;
                        var data = GetDoctorData(formPage.data);
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("Is this the time you booked?");
                        foreach (var p in data)
                        {
                            msg.AppendLine(p.Key + ": " + p.Value);
                        }
                        new AlertDialog.Builder(this).SetTitle("Please check your information").SetMessage(msg.ToString()).SetPositiveButton("Yes", (sender, args) =>
                         {
                             Toast.MakeText(this, "Notifying doctors...", ToastLength.Long).Show();
                             if (NotifyDoctors(formPage.data))
                             {
                                 Toast.MakeText(this, "Doctors received", ToastLength.Short).Show();
                                 new AlertDialog.Builder(this).SetTitle("Great!").SetMessage("Doctors now know you've checked in, please be patient and wait for notification.").SetPositiveButton("Got it!", (sender1, args1) =>
                                {
                                     SetPage(logoPage);
                                 }).Show();
                             }
                             else
                             {
                                 new AlertDialog.Builder(this).SetTitle("No no!").SetMessage("Problems occurred when notifying doctors, please try again >_<").SetPositiveButton("All right",(sender1,args1)=>
                                 {
                                 }).Show();
                             }
                         }).SetNegativeButton("No", (sender, args) =>
                         {
                             new AlertDialog.Builder(this).SetTitle("What you can do").SetMessage("1. Review the form you input\r\n2. Ask any staff for help").SetPositiveButton("OK", (sender1, args1) =>
                             {
                             }).Show();
                         }).Show();
                        formPage.Enabled = true;
                    };
                  Toast.MakeText(this, string.Format("Initializing stage {0}...", ++initializingStage), ToastLength.Short).Show();
              };
            SetPage(logoPage);
            Toast.MakeText(this, "Completed", ToastLength.Short).Show();
        }
    }
}

