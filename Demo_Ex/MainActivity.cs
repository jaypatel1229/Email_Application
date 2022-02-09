using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace Demo_Ex
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainAcivity : AppCompatActivity
    {
        private EditText _toEditText;
        private EditText _ccEditText;
        private EditText _bccEditText;
        private EditText _subjectEditText;
        private EditText _bodyEditText;
        private Button _sendEmailButton;
        private Button _addAttechmentButton;
        private List<string> _recipientsList;
        private List<string> _ccList;
        private List<string> _bccList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReferences();
            UIClickEvents();
        }

        private void UIClickEvents()
        {
            _sendEmailButton.Click += SendEmailButton_Click;
            _addAttechmentButton.Click += AddAttechmentButton_Click;
        }

        private async void AddAttechmentButton_Click(object sender, EventArgs e)
        {
            EmailMessage emailMessage = GetEmailMessage();
            var fn = "Delhi.jpg";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            //File.WriteAllText(file, "");
            emailMessage.Attachments.Add(new EmailAttachment(file));
            try
            {
                await Email.ComposeAsync(emailMessage);
            }
            catch (FeatureNotSupportedException exception)
            {
                Toast.MakeText(this, exception.Message, ToastLength.Short).Show();
            }
            catch (Exception exception)
            {
                Toast.MakeText(this, exception.Message, ToastLength.Short).Show();
            }
        }

        private async void SendEmailButton_Click(object sender, EventArgs e)
        {
            EmailMessage emailMessage = GetEmailMessage();
            await Email.ComposeAsync(emailMessage);
        }

        private EmailMessage GetEmailMessage()
        {
            _recipientsList = new List<string>();
            _ccList = new List<string>();
            _bccList = new List<string>();
            _recipientsList.AddRange(_toEditText.Text.Trim().Split(separator: " "));
            _ccList.AddRange(_ccEditText.Text.Trim().Split(separator: " "));
            _bccList.AddRange(_bccEditText.Text.Trim().Split(separator: " "));
            string subject = _subjectEditText.Text;
            string body = _bodyEditText.Text;

            EmailMessage emailMessage = new EmailMessage
            {
                To = _recipientsList,
                Cc = _ccList,
                Bcc = _bccList,
                Subject = subject,
                Body = body
            };
            return emailMessage;
        }
        private void UIReferences()
        {


            _toEditText = FindViewById<EditText>(Resource.Id.toedit);
            _subjectEditText = FindViewById<EditText>(Resource.Id.subedit);
            _bodyEditText = FindViewById<EditText>(Resource.Id.msgedit);
            _ccEditText = FindViewById<EditText>(Resource.Id.ccedit);
            _bccEditText = FindViewById<EditText>(Resource.Id.bccedit);

            _sendEmailButton = FindViewById<Button>(Resource.Id.sendbtn);
            _addAttechmentButton = FindViewById<Button>(Resource.Id.attachbtn);
        }
    }
}
