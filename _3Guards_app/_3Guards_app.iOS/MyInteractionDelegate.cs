using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace _3Guards_app.iOS
{
    public class MyInteractionDelegate : UIDocumentInteractionControllerDelegate
    {
        UIViewController parent;

        public MyInteractionDelegate(UIViewController controller)
        {
            parent = controller;
        }

        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
        {
            return parent;
        }
    }
}