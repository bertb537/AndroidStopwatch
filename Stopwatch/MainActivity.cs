using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Threading.Tasks;

namespace Stopwatch
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string TAG_COUNT = "COUNT";
        private string TAG_START_FLAG = "START_FLAG";

        private bool FLAG_START = false;

        private int count = 0;
        private bool saveStartState = false;

        TextView hourText;
        TextView minuteText;
        TextView secondText;

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(TAG_COUNT, count);
            outState.PutBoolean(TAG_START_FLAG, FLAG_START);
        }

        protected override void OnStop()
        {
            base.OnStop();

            saveStartState = FLAG_START;
            FLAG_START = false;
        }

        protected override void OnStart()
        {
            base.OnStart();

            FLAG_START = saveStartState;
            saveStartState = false;

            if(FLAG_START)
            {
                Stopwatch();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Init references to the UI resources
            Button startBtn = FindViewById<Button>(Resource.Id.start_button);
            Button stopBtn = FindViewById<Button>(Resource.Id.stop_button);
            Button resetBtn = FindViewById<Button>(Resource.Id.reset_button);

            hourText = FindViewById<TextView>(Resource.Id.hours);
            minuteText = FindViewById<TextView>(Resource.Id.minutes);
            secondText = FindViewById<TextView>(Resource.Id.seconds);

            // Retrieve Saved Instance Data
            if(savedInstanceState != null)
            {
                count = savedInstanceState.GetInt(TAG_COUNT, 0);
                FLAG_START = savedInstanceState.GetBoolean(TAG_START_FLAG, false);
            }

            // Assign Delegates
            startBtn.Click += delegate
            {
                FLAG_START = true;

                // Start StopWatch Async
                Stopwatch();
            };

            stopBtn.Click += delegate
            {
                FLAG_START = false;
            };

            resetBtn.Click += delegate
            {
                count = 0;

                hourText.Text = "00";
                minuteText.Text = "00";
                secondText.Text = "00";
            };
        }

        private async void Stopwatch()
        {
            while(FLAG_START)
            {
                int hours = count / 3600;
                int minutes = (count % 3600) / 60;
                int seconds = count % 60;

                hourText.Text = Convert.ToString(hours / 10) + Convert.ToString(hours % 10);
                minuteText.Text = Convert.ToString(minutes / 10) + Convert.ToString(minutes % 10);
                secondText.Text = Convert.ToString(seconds / 10) + Convert.ToString(seconds % 10);

                await Task.Delay(1000);

                ++count;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}