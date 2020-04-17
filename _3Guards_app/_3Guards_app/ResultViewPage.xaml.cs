using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultViewPage : ContentPage
    {
       

        public ResultViewPage()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var result = (Result)BindingContext;
            var test = result.ID;
            var test1 = result.Name;


            listView.ItemsSource = await App.Database.GetTimingsAsync(result.ID);

            Conducting.Source = GetFromDisk(result.ConductingSig);
            Supervising.Source = GetFromDisk(result.SupervisingSig);
            Safety.Source = GetFromDisk(result.SafetySig);

        }
        private static ImageSource GetFromDisk(string imageFileName)
        {
            var imageAsBase64String = Xamarin.Essentials.Preferences.Get(imageFileName, string.Empty);

            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var result = (Result)BindingContext;
            await App.Database.DeleteResultAsync(result);
            await Navigation.PopAsync();

        }
    }
}