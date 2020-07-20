using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using PdfSharpCore.Pdf;
using UIKit;

namespace _3Guards_app.iOS
{
    class PdfSave:IPdfSave
    {
        public void Save(PdfDocument doc, string fileName)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, fileName);

            doc.Save(path);
            doc.Close();

            global::Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                title: "Success",
                message: $"Your PDF generated and saved in Documents",
                cancel: "OK");
        }
    }
}