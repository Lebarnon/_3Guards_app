using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;

namespace _3Guards_app.iOS
{
    class PdfOpen : IPdfOpen
    {
        public void Open(string fileName)
        {
            //var PreviewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(filePath));
            //PreviewController.Delegate = new UIDocumentInteractionControllerDelegate(UIApplication.SharedApplication.KeyWindow.RootViewController);
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    PreviewController.PresentPreview(true);
            //});


            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, fileName);
            var previewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(path));

            previewController.Delegate = new MyInteractionDelegate(UIApplication.SharedApplication.KeyWindow.RootViewController);
            previewController.PresentPreview(true);
        }
    }
}