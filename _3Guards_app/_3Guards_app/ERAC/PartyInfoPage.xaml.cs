using _3Guards_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _3Guards_app.ERAC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartyInfoPage : ContentPage
    {
       

        public PartyInfoPage()
        {
            InitializeComponent();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            EracUser user = (EracUser)BindingContext;
            user.Name = Name.Text.ToUpper();
            user.Nric = NRIC.Text.ToUpper();
            user.Rank = Rank.Text.ToUpper();
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}