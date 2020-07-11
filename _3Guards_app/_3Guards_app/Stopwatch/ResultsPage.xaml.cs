using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using _3Guards_app.Data;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetResultsAsync();
        }

       
        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new ResultViewPage
                {
                    BindingContext = e.SelectedItem as Result
                }) ;
            }
        }
        async void OnDeleteAllButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Complete Reset", "All results will be permenantly deleted", "Yes", "No");
            if (answer == true)
            {
                App.Database.Reset();
            }
            else
            {
                return;
            }

            RefreshList();
        }
        async void OnItemDeleteClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete", "Selected Result will be permenantly deleted", "Yes", "No");
            if (answer == true)
            {
                // The sender is the menuItem
                MenuItem menuItem = sender as MenuItem;

                // Access the list item through the BindingContext
                var result = (Result)menuItem.BindingContext;
                await App.Database.DeleteResultAsync(result);

                RefreshList();
            }
            else
            {
                return;
            }
           

        }
        async void RefreshList() 
        {
            listView.ItemsSource = null;
            listView.ItemsSource = await App.Database.GetResultsAsync();
        }
    }
}