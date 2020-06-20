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

namespace _3Guards_app.Droid
{
    [Activity(Label = "3Guards", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

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
    }
}