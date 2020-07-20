using _3Guards_app.Droid;
using Android.OS;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(PdfOpen))]
namespace _3Guards_app.Droid
{
    class PdfOpen:IPdfOpen
    {
        
        public void Open(string fileName)
        {
            MainActivity.GetInstance().PdfOpen(fileName);
        }   
    }
}