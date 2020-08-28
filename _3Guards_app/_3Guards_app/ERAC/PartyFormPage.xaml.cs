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
        private void OnAddPartyClicked(object sender, EventArgs e)
        {

            // c = CURRENT
            int cGridrow = 1;
            int cGridcol = 0;
            if(cGridcol > 3)
            {
                cGridcol = 0;
                return;
            }

            string cRank = cGridcol.ToString() + cGridrow.ToString() + "NRIC";
            string cName = cGridcol.ToString() + cGridrow.ToString() + "NRIC";
            string cNRIC = cGridcol.ToString() + cGridrow.ToString() + "NRIC";

            Participants.Children.Add(new Entry {
                Placeholder = "hi", 
            }, 1, 0) ;

            
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new RacForm());
        }
    }
}