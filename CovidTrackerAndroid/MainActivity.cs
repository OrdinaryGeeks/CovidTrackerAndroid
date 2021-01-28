using Android;
using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Threading.Tasks;


namespace CovidTrackerAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener, ILocationListener
    {
        TextView textMessage;
        Random random;
        RelativeLayout layout;
        LocationManager locationManager;
        TelephonyManager telephoneMGR;

        protected  override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);


            SetContentView(Resource.Layout.activity_main);


            Android.Content.PM.Permission permission = ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera);
            layout = (RelativeLayout)FindViewById(Resource.Id.container);

            bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
            bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);


            if (CheckingPermissionIsEnabledOrNot())
                {
                    Snackbar.Make(layout, "All Permissions Granted Successfully", Snackbar.LengthIndefinite).Show();
                }
            else
            {

                RequestMultiplePermissions();
            }



            telephoneMGR = (TelephonyManager)GetSystemService(TelephonyService);

            string number = telephoneMGR.Line1Number;


                textMessage = FindViewById<TextView>(Resource.Id.message);


            locationManager = (LocationManager)GetSystemService(LocationService);

            if (locationManager.AllProviders.Contains(LocationManager.NetworkProvider)
&& locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                //     getLastLocationButton.Click += GetLastLocationButtonOnClick;
                //   requestLocationUpdatesButton.Click += RequestLocationUpdatesButtonOnClick;
            }
            else
            {
                Snackbar.Make(layout, "nogps", Snackbar.LengthIndefinite)
                        .SetAction("ok", delegate { FinishAndRemoveTask(); })
                        .Show();
            }

            long time = 3000;

            
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, time, .02f, this);

            random = new Random(DateTime.Now.Second);

           //  SaveCountAsync(random.Next());
            textMessage.Text =  GetText();
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        private void RequestMultiplePermissions()
        {

            // Creating String Array with Permissions.
            ActivityCompat.RequestPermissions(this, new String[]
                    {

          Manifest.Permission.Camera,
                     Manifest.Permission.AccessFineLocation,
                     Manifest.Permission.AccessCoarseLocation,
                   Manifest.Permission.WriteExternalStorage,
                   Manifest.Permission.ReadPhoneState
                    }, 1003);

        }


        public void OnLocationChanged(Android.Locations.Location location)
        {
           
            double oldThreshold = .1;
          textMessage.Text=   GetLastLocationFromDevice();
        //   WriteLocationAndDateTime()

        }

        public bool CheckingPermissionIsEnabledOrNot()
        {

            Permission FirstPermissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera);
            Permission SecondPermissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
            Permission ThirdPermissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);
            Permission ForthPermissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage);
            Permission FifthPermissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState);

            return FirstPermissionResult == Permission.Granted &&
                    SecondPermissionResult == Permission.Granted &&
                    ThirdPermissionResult == Permission.Granted &&
                    ForthPermissionResult == Permission.Granted &&
                    FifthPermissionResult == Permission.Granted;
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {
            // Nothing to do in this example.
            Log.Debug("LocationExample", "The provider " + provider + " is enabled.");
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            if (status == Availability.OutOfService)
            {
   
            }
        }
        public string GetLastLocationFromDevice()
        {
           // textMessage.SetText(Resource.String.getting_last_location);

            var criteria = new Criteria { PowerRequirement = Power.Medium };

            var bestProvider = locationManager.GetBestProvider(criteria, true);
            var location = locationManager.GetLastKnownLocation(bestProvider);

         string locAndTime = GetLocationAndTime();

            if (location != null)
            {
              //  textMessage.Text =(location.ToString());
               WriteLocationAndDateTime(location.Latitude.ToString() + " " + location.Longitude.ToString() + " " + DateTime.Now.ToString() +  '\n');
                /*latitude.Text = Resources.GetString(Resource.String.latitude_string, location.Latitude);
                longitude.Text = Resources.GetString(Resource.String.longitude_string, location.Longitude);
                provider.Text = Resources.GetString(Resource.String.provider_string, location.Provider);
                getLastLocationButton.SetText(Resource.String.get_last_location_button_text);*/
            }
            else
            {
              //  textMessage.Text = (location.ToString());
                /*
                latitude.SetText(Resource.String.location_unavailable);
                longitude.SetText(Resource.String.location_unavailable);
                provider.Text = Resources.GetString(Resource.String.provider_string, bestProvider);
                getLastLocationButton.SetText(Resource.String.get_last_location_button_text);
                */
            }
            return locAndTime;
        }


        public void callSaveCountAsync()
        {


       //   await SaveCountAsync(5);
        }


        public void WriteLocationAndDateTime(string input)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locationAndTimes2.txt");


            using (var writer = File.AppendText(backingFile))
            {
                  writer.WriteLineAsync(input);
            }


        }
        public  void SaveCountAsync(int count)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

          
            using (var writer = File.AppendText(backingFile))
            {
                  writer.WriteLineAsync(GetLastLocationFromDevice());
            }
        }

        public string GetLocationAndTime()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "locationAndTimes2.txt");

            if(File.Exists(filename))
            using (var streamReader = new StreamReader(filename))
            {
                string content = streamReader.ReadToEnd();
                return content;
            }
            return "0";
        }


        public string  GetText()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "count.txt");
            using (var streamReader = new StreamReader(filename))
            {
                string content = streamReader.ReadToEnd();
                return content;
            }
            return "0";
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            string TAG = "MainActivity";
            if (requestCode == 1000)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for Camera permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "Camera permission has now been granted.");
                    Snackbar.Make(layout,"Camera", Snackbar.LengthShort).Show();
                }
                else
                {
                    Log.Info(TAG, "Camera permission was NOT granted.");
                    Snackbar.Make(layout, "nope", Snackbar.LengthShort).Show();
                }
            }
            else if(requestCode == 1001)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for FLocation permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location FLocation has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "Location permission has now been granted.");
                    Snackbar.Make(layout, "FLocation", Snackbar.LengthShort).Show();
                }
                else
                {
                    Log.Info(TAG, "Location permission was NOT granted.");
                    Snackbar.Make(layout, "nope", Snackbar.LengthShort).Show();
                }
            }
            else if(requestCode == 1002)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for CLocation permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "CLocation permission has now been granted.");
                    Snackbar.Make(layout, "CLocation", Snackbar.LengthShort).Show();
                }
                else
                {
                    Log.Info(TAG, "CLocation permission was NOT granted.");
                    Snackbar.Make(layout, "nope", Snackbar.LengthShort).Show();
                }
            }
            else if(requestCode == 1003)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for External permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "External permission has now been granted.");
                    Snackbar.Make(layout, "External", Snackbar.LengthShort).Show();
                }
                else
                {
                    Log.Info(TAG, "External permission was NOT granted.");
                    Snackbar.Make(layout, "nope", Snackbar.LengthShort).Show();
                }
            }

                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

