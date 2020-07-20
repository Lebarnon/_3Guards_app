using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Environment = Android.OS.Environment;
using Android.Content;
using Java.IO;

using Xamarin.Essentials;
using Android.Views;
using System.Linq;
using _3Guards_app;
using PdfSharpCore.Pdf;

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


            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            PdfSharp.Xamarin.Forms.Droid.Platform.Init();

            CheckAppPermissions();

            LoadApplication(new App());
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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

        public static MainActivity GetInstance()
        {
            return Instance;
        }
        public void PdfOpen(string fileName)
        {
            string path = System.IO.Path.Combine(GetExternalFilesDir(Environment.DirectoryDocuments).AbsolutePath + "/" + fileName);

            File file = new File(path);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            
            var uri = Xamarin.Essentials.FileProvider.GetUriForFile(Instance, Application.Context.PackageName + ".provider", file);
            intent.SetDataAndType(uri, "application/pdf");
            StartActivity(intent);
            //Launcher.OpenAsync(path);
        }

        public void PdfSave(PdfDocument doc, string fileName)
        {
            string path = System.IO.Path.Combine(GetExternalFilesDir(Environment.DirectoryDocuments).AbsolutePath + "/" + fileName);

            doc.Save(path);
            doc.Close();

            global::Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                title: "Success",
                message: $"Your PDF generated and saved in Documents",
                cancel: "OK");

        }

    }
}