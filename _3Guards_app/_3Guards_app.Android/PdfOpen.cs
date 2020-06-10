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
using Java.IO;
using Java.Nio.FileNio;

namespace _3Guards_app.Droid
{
    class PdfOpen
    {
        public void Open(string pathName)
        {
            File file = new File(pathName);
            Files.NewInputStream(pathName, );
        }
        public static void OpenFile(Activity activity, String name, string pathName)
        {
            File file = new File(pathName);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(file.ToURI(), "application/pdf");
            
            activity.StartActivity(intent);
        }
    }
}