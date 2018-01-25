using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System.Threading;
using Android.Telephony;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace InformThatJerks
{
    [Activity(Label = "Inform That Jerks v.1.0", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, INotifyPropertyChanged
    {
        SmsManager smsManager;
        Button btSwitchService, btExit;
        EditText editNumber, editMessage;

        public string PhoneNum { get; set; }
        public string Message { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
            }

            btSwitchService = FindViewById<Button>(Resource.Id.btSwitchService);
            btExit = FindViewById<Button>(Resource.Id.btExit);

            editNumber = FindViewById<EditText>(Resource.Id.editNumber);
            editMessage = FindViewById<EditText>(Resource.Id.editMessage);

            btExit.Click += delegate
            {
                 Toast.MakeText(this,"FORK YOU", ToastLength.Short).Show();
                 Thread.Sleep(1000);
                 System.Environment.Exit(-1);
            };
        }
        void SendSMS(string number, string message)
        {
            List<PendingIntent> sentPendingIntents = new List<PendingIntent>();
            List<PendingIntent> deliveredPendingIntents = new List<PendingIntent>();
            PendingIntent sentPI = PendingIntent.GetBroadcast(BaseContext, 0,
                        new Intent(BaseContext, typeof(SmsSentReceiver)), 0);
            PendingIntent deliveredPI = PendingIntent.GetBroadcast(BaseContext, 0,
                    new Intent(BaseContext, typeof(SmsDeliveredReceiver)), 0);
            try
            {
                SmsManager sms = SmsManager.Default;
                List<string> mSMSMessage = sms.DivideMessage(message).ToList();
                for (int i = 0; i<mSMSMessage.Count; i++) {
                    sentPendingIntents.Add(sentPI);
                    deliveredPendingIntents.Add(deliveredPI);
                }
                sms.SendMultipartTextMessage(number, null, mSMSMessage,
                        sentPendingIntents, deliveredPendingIntents);
            }
            catch (Exception e)
            {
                Toast.MakeText(BaseContext, "SMS sending failed... I don't get a fuck why, I'm just an application. Contact the developer.",ToastLength.Short).Show();
            }    
        }
    }
    public class SmsSentReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            throw new NotImplementedException();
        }
    }
    public class SmsDeliveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}

