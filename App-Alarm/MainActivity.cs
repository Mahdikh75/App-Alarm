using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace App_Alarm
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            StartService(new Intent(this, typeof(AlarmService)));

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new string[] { "android.intent.BOOT_COMPLETED" })]
    public class Alarm : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context , "Alarm ...", ToastLength.Long).Show();
            Vibrator vibrator = context.GetSystemService(Context.VibratorService) as Vibrator;
            vibrator.Vibrate(2000);
        }

        public void SetTime(Context context , int time)
        {
            AlarmManager alarm = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, typeof(Alarm));
            PendingIntent pending = PendingIntent.GetBroadcast(context, 0, intent, 0);
            alarm.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + time, pending); // base time + time me (ms)
        }

        public void Cancel(Context context)
        {
            AlarmManager alarm = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, typeof(Alarm));
            PendingIntent pending = PendingIntent.GetBroadcast(context, 0, intent, 0);
            alarm.Cancel(pending);
        }

    }

    [Service (Enabled = true)]
    public class AlarmService : Service
    {
        Alarm alarm = new Alarm();
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Toast.MakeText(this, "Service Start", ToastLength.Long).Show();
            alarm.SetTime(this, 10000);
            return base.OnStartCommand(intent, flags, startId);
        }
    }

}