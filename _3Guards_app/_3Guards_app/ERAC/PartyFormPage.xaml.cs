using _3Guards_app.ERAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using System.Collections.ObjectModel;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartyFormPage : ContentPage
    {
        //var result = (Result)BindingContext;

        //For display of timing in stopwatch
        ObservableCollection<EracUser> eracUsers = new ObservableCollection<EracUser>();
        public ObservableCollection<EracUser> EracUsers { get { return eracUsers; } }
        //
        List<EracUser> listOfUser = new List<EracUser>();

        public PartyFormPage()
        {
            InitializeComponent();
        }
        private async void OnAddPartyClicked(object sender, EventArgs e)
        {
            EracUser user = new EracUser();
            await Navigation.PushModalAsync(new PartyInfoPage { BindingContext = user as EracUser });
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < eracUsers.Count; i++)
            {
                await App.Database.SaveEracUserAsync(listOfUser[i]);
            }

            await Navigation.PushAsync(new RacForm());
        }
        public async void CheckUser(EracUser user)
        {
            if(user.Name == null || user.Nric == null|| user.Rank == null)
            {
                await DisplayAlert("Failed", "", "Ok");
            }
            else
            {
                eracUsers.Add(user);
                listOfUser.Add(user);
            }
        }
    }
}