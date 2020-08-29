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

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            if (Q1.IsEnabled == false || Q2.IsEnabled == false || Q3.IsEnabled == false || Q4.IsEnabled == false || Q5.IsEnabled == false || Q6.IsEnabled == false || Q7.IsEnabled == false || Q8.IsEnabled == false || Q9.IsEnabled == false || Q10.IsEnabled == false)
            {
                DisplayPromptAsync("Warning", "Please sound off to the Conductin/Safety for questions that is not checked", "OK");
            }
        }
    }
}