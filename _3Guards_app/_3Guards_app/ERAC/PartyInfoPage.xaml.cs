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
    public partial class PartyInfoPage
    {
        //var result = (Result)BindingContext;
        public PartyInfoPage()
        {
            InitializeComponent();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if(Name.Text == null || NRIC.Text == null || Rank.Text == null)
            {
                await DisplayAlert("Missing Information", "Some Fields are not filled in", "Ok");
                return;
            }
            else
            {
                
                Erac erac = (Erac)BindingContext;
                EracUser user = new EracUser();
                user.Name = Name.Text.ToUpper();
                user.Nric = NRIC.Text.ToUpper();
                user.Rank = Rank.Text.ToUpper();
                SaveUserandLinktoErac(erac, user);
                await Navigation.PopModalAsync();
            }
            
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SaveUserandLinktoErac(Erac erac, EracUser user)
        {
            await App.Database.SaveEracUserAsync(user);
            erac.EracUsers.Add(user);
            await App.Database.PopulateEracEracQues(erac);
        }
    }
}