using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultViewPage : ContentPage
    {
        public ResultViewPage()
        {
            InitializeComponent();
        }


        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var result = (Result)BindingContext;
            await App.Database.DeleteResultAsync(result);
            await Navigation.PopAsync();

        }
    }
}