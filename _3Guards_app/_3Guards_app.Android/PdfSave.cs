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
			string path = System.IO.Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath + "/" +fileName);

			doc.Save(path);
			doc.Close();
			
			global::Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
				title: "Success",
				message: $"Your PDF generated and saved in Documents",
				cancel: "OK");
			
		}
	}
}