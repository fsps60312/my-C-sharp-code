using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Client6
{
    public class FormPage : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public Button back, ok;
        public Dictionary<string, string> data=new Dictionary<string, string>();
        Tuple<int, string>[] textIds = new Tuple<int, string>[]
        {
            new Tuple<int, string>(Resource.Id.firstName,"First name"),
            new Tuple<int, string>(Resource.Id.surname,"Surname"),
            new Tuple<int, string>(Resource.Id.birthday,"Birthday"),
            new Tuple<int, string>(Resource.Id.medicareNumber,"Medicare number")
        };
        View v;
        public EventHandler EventsEnabled;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            v = inflater.Inflate(Resource.Layout.formPage, container, false);
            back = v.FindViewById<Button>(Resource.Id.back);
            ok = v.FindViewById<Button>(Resource.Id.ok);
            foreach (var t in textIds)
            {
                EditText e = v.FindViewById<EditText>(t.Item1);
                e.TextChanged += delegate
                {
                    data[t.Item2] = e.Text;
                };
            }
            ClearTexts();
            EventsEnabled?.Invoke(this, null);
            return v;
        }
        public void ClearTexts()
        {
            foreach (var t in textIds)
            {
                EditText e = v.FindViewById<EditText>(t.Item1);
                data[t.Item2] = e.Text = "";
            }
        }
        bool _Enabled=true;
        public bool Enabled
        {
            set
            {
                if (value == _Enabled) return;
                back.Enabled = value;
                ok.Enabled = value;
                foreach (var t in textIds)
                {
                    EditText e = v.FindViewById<EditText>(t.Item1);
                    e.Enabled = value;
                }
                _Enabled = value;
            }
            get
            {
                return _Enabled;
            }
        }
    }
}