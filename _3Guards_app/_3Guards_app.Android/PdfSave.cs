using _3Guards_app.Droid;
using Android.OS;
using Android.Views.Accessibility;
using PdfSharpCore.Pdf;

[assembly: Xamarin.Forms.Dependency(typeof(PdfSave))]
namespace _3Guards_app.Droid
{
	public class PdfSave : IPdfSave
	{
		public void Save(PdfDocument doc, string fileName)
		{
			MainActivity.GetInstance().PdfSave(doc, fileName);
		}
	}
}