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

        private async void AddCommand(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}