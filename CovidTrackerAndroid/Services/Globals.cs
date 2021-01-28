using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace CovidTrackerAndroid.Services
{
    public class Globals
    {

        public  AlectoDataStore AlectoDataStore;//= new AlectoDataStore();

        public Globals()
        {

            AlectoDataStore = new AlectoDataStore();
        }
    }
}