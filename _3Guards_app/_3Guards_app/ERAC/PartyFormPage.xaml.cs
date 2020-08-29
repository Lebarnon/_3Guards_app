using _3Guards_app.ERAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartyFormPage : ContentPage
    {
        public PartyFormPage()
        {
            InitializeComponent();
        }
        private async void OnAddPartyClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PartyInfoPage());
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new RacForm());
        }
    }
}