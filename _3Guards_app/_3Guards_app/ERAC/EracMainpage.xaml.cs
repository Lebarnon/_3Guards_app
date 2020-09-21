using _3Guards_app.Models;
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
    public partial class EracMainpage : ContentPage
    {
        public EracMainpage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetEracsAsync();
        }
        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (e.SelectedItem != null)
            //{
            //    await Navigation.PushAsync(new ResultViewPage
            //    {
            //        BindingContext = e.SelectedItem as Result
            //    });
            //}
        }
        
        async void OnNewRACClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("New RAC", "Create a new RAC?", "Yes", "No");
            string resultname = await DisplayPromptAsync("Activity", "", placeholder: "Name of Activity");
            if (answer == true && resultname != null)
            {
                Erac erac = new Erac();
                await Navigation.PushAsync(new PartyFormPage
                {
                    BindingContext = erac as Erac
                });
            }
            else
            {
                return;
            }
        }
        async void OnItemDeleteClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete", "Selected Result will be permenantly deleted", "Yes", "No");
        }
    }
}