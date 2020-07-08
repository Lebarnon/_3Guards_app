using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using SignaturePad.Forms;


namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Signature : ContentPage
    {

       
        public Signature()
        {
            InitializeComponent();
        }

        private async void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            var result = (Result)BindingContext;
           
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
            await SaveSig(ConductingSig, result);
            await SaveSig(SupervisingSig, result);
            await SaveSig(SafetySig, result);
            activityIndicator.IsRunning = false;
            
            await App.Database.SaveResultAsync(result);
            var res = await App.Database.GetResultsAsync();
            Navigation.RemovePage(this);

        }

        public async Task SaveSig(SignaturePadView whichSignature, Result result)
        {
            Stream bitmap =   await whichSignature.GetImageStreamAsync(SignatureImageFormat.Png);
            string sigOwner = whichSignature.CaptionText;

            string tempSigFileName = result.Name + " " + sigOwner + " " + result.DateCreated;

            byte[] sigBit = ReadFully(bitmap);
            SaveToDisk(tempSigFileName, sigBit);

            if( sigOwner == "Conducting")
            {
                result.ConductingSig = tempSigFileName;
            }
            else if (sigOwner == "Supervising")
            {
                result.SupervisingSig = tempSigFileName;
            }
            else if (sigOwner == "Safety")
            {
                result.SafetySig = tempSigFileName;
            }
        }

       
        public static void SaveToDisk(string imageFileName, byte[] imageAsBase64String)
        {
            Xamarin.Essentials.Preferences.Set(imageFileName, Convert.ToBase64String(imageAsBase64String));
        }
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        
    }
}