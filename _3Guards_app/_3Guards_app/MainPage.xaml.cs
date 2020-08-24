using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using _3Guards_app.Models;

namespace _3Guards_app
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void OnStopwatchPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StopwatchPage());
            //App.Database.CheckTables();
        }

        async void OnResultsPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultsPage());
        }
        async void OnSafetyPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RacForm());
        }
        async void OnEmptyClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Sorry!", "This feature will be available in the future", "OK");
        }
    }
}
