using _3Guards_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacForm : ContentPage
    {
        public RacForm()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var erac = (Erac)BindingContext;
            List<EracUser> eracUsers = App.Database.GetEracEracUserList(erac.ID).Result;
            string participants = "";

            for (int i = 0; i < eracUsers.Count; i++)
            {
                participants += eracUsers[i].Rank + eracUsers[i].Name + ", ";
            }

           
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            if (Q1.IsEnabled == false || Q2.IsEnabled == false || Q3.IsEnabled == false || Q4.IsEnabled == false || Q5.IsEnabled == false || Q6.IsEnabled == false || Q7.IsEnabled == false || Q8.IsEnabled == false || Q9.IsEnabled == false || Q10.IsEnabled == false)
            {
                await DisplayPromptAsync("SOUND OFF", "Please sound off to the Conducting/Safety", "OK");
            }
            else
            {
                var erac = (Erac)BindingContext;
                EracQues eracQ = new EracQues();
                eracQ.Q1 = true;
                eracQ.Q2 = true;
                eracQ.Q3 = true;
                eracQ.Q4 = true;
                eracQ.Q5 = true;
                eracQ.Q6 = true;
                eracQ.Q7 = true;
                eracQ.Q8 = true;
                eracQ.Q9 = true;
                eracQ.Q10 = true;

                erac.EracQuess = eracQ;
                await App.Database.SaveEracAsync(erac);
                await App.Database.PopulateEracEracQues(erac);
                await Navigation.PopAsync();
            }
        }
    }
}