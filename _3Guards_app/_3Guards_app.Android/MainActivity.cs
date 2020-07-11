using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Environment = Android.OS.Environment;
using Android.Content;
using Java.IO;
using Android.Support.V4.Content;
using Xamarin.Essentials;
using Android.Views;
using System.Linq;
using _3Guards_app;

namespace _3Guards_app.Droid
{
    [Activity(Label = "3Guards", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            PdfSharp.Xamarin.Forms.Droid.Platform.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CheckAppPermissions();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }

        public static MainActivity getInstance()
        {
            return Instance;
        }
        public void PdfOpen(string fileName)
        {
            string path = System.IO.Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath + "/" + fileName);

            File file = new File(path);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            var uri = Xamarin.Essentials.FileProvider.GetUriForFile(Instance, Application.Context.PackageName + ".provider", file);
            intent.SetDataAndType(uri, "application/pdf");
            StartActivity(intent);
            //Launcher.OpenAsync(path);
        }       
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    // check if the current item id 
        //    // is equals to the back button id
        //    if (item.ItemId == 16908332)
        //    {
        //        // retrieve the current xamarin forms page instance
        //        var currentpage = (StopBackContentPage)
        //        Xamarin.Forms.Application.
        //        Current.MainPage.Navigation.
        //        NavigationStack.LastOrDefault();

        //        // check if the page has subscribed to 
        //        // the custom back button event
        //        if (currentpage?.CustomBackButtonAction != null)
        //        {
        //            // invoke the Custom back button action
        //            currentpage?.CustomBackButtonAction.Invoke();
        //            // and disable the default back button action
        //            return false;
        //        }

        //        // if its not subscribed then go ahead 
        //        // with the default back button action
        //        return base.OnOptionsItemSelected(item);
        //    }
        //    else
        //    {
        //        // since its not the back button 
        //        //click, pass the event to the base
        //        return base.OnOptionsItemSelected(item);
        //    }
        //}

        //public override void OnBackPressed()
        //{
        //    // this is not necessary, but in Android user 
        //    // has both Nav bar back button and
        //    // physical back button its safe 
        //    // to cover the both events

        //    // retrieve the current xamarin forms page instance
        //    var currentpage = (StopBackContentPage)
        //    Xamarin.Forms.Application.
        //    Current.MainPage.Navigation.
        //    NavigationStack.LastOrDefault();

        //    // check if the page has subscribed to 
        //    // the custom back button event
        //    if (currentpage?.CustomBackButtonAction != null)
        //    {
        //        currentpage?.CustomBackButtonAction.Invoke();
        //    }
        //    else
        //    {
        //        base.OnBackPressed();
        //    }
        //}
    }
}