using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using Xamarin.Essentials;
using System.IO;

namespace _3Guards_app.Stopwatch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignatureViewPage : ContentPage
    {
        public SignatureViewPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var result = (Result)BindingContext;

            Conducting.Source = GetFromDisk(result.ConductingSig);
            Supervising.Source = GetFromDisk(result.SupervisingSig);
            Safety.Source = GetFromDisk(result.SafetySig);
        }
        private static ImageSource GetFromDisk(string imageFileName)
        {
            var imageAsBase64String = Preferences.Get(imageFileName, string.Empty);

            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
        }
    }
}