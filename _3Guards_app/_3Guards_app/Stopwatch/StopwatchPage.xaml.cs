using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _3Guards_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Markup;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopwatchPage : ContentPage
    {   
        readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        Result result = new Result();
        private int timingID = 0;
        List<Timing> ListOfTimings = new List<Timing>();

        //For display of timing in stopwatch
        ObservableCollection<DisplayTiming> displayTimings = new ObservableCollection<DisplayTiming>();
        public ObservableCollection<DisplayTiming> DisplayTimings { get { return displayTimings; } }
        //
       
        public class DisplayTiming
        {
            public string Duration { get; set; }
        }
        public static Timing FactoryOfTiming(string timing)
        {
            return new Timing() {
                Time = timing
            };
        }

        public StopwatchPage()
        {
            InitializeComponent();
            
            //stopwatch = new System.Diagnostics.Stopwatch();

            //only start button
            btnLapReset.IsVisible = false;
            btnStartStop.SetValue(Grid.ColumnSpanProperty, 2);

            DisplayTimingsView.ItemsSource = displayTimings;
           

            lblStopwatch.Text = "00:00.00";
        }
        public void BtnStartStop_Clicked(object sender, EventArgs e)
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                //
                btnStartStop.Text = "Stop";
                btnStartStop.SetValue(Grid.ColumnSpanProperty, 1);
                btnStartStop.SetValue(Grid.ColumnProperty, 0);
                btnStartStop.TextColor = Color.White;
                btnStartStop.BackgroundColor = Color.FromHex("#a60000");

                btnLapReset.Text = "Lap";
                btnLapReset.TextColor = Color.Black;
                btnLapReset.BackgroundColor = Color.White;
                btnLapReset.IsVisible = true;
                btnLapReset.SetValue(Grid.ColumnProperty, 1);

                btnSave.IsVisible = false;

                Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
                {
                    lblStopwatch.Text = stopwatch.Elapsed.ToString(@"mm\:ss\.ff");

                    if (!stopwatch.IsRunning)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                );
                DisplayAlert("Caution", "Please do not return to the main page as the timer will reset", "OK");
            }
            else if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                btnStartStop.Text = "Resume";
                btnStartStop.TextColor = Color.Black;
                btnStartStop.BackgroundColor = Color.White;

                btnLapReset.Text = "Reset";
                btnLapReset.TextColor = Color.White;
                btnLapReset.BackgroundColor = Color.FromHex("#a60000");

                if (timingID < 1)
                {
                    btnSave.IsVisible = false;
                }
                else{
                    btnSave.IsVisible = true;
                }
            }
        }
        private async void BtnLapReset_Clicked(Object sender, EventArgs e)
        {
            if (!stopwatch.IsRunning)
            {
                bool answer = await DisplayAlert("Reset", "Timings in this session will be permenantly deleted", "Yes", "No");
                if(answer == false)
                {
                    return;
                }
                else if(answer == true)
                {
                    // Display Update
                    stopwatch.Reset();
                    DisplayTimings.Clear();

                    btnLapReset.IsVisible = false;
                    btnStartStop.SetValue(Grid.ColumnSpanProperty, 2);
                    btnStartStop.TextColor = Color.Black;
                    btnStartStop.BackgroundColor = Color.White;
                    btnStartStop.Text = "Start";

                    lblStopwatch.Text = "00:00.00";

                    btnSave.IsVisible = false;

                    // DataUpdate
                    timingID = 0;
                }
            }
            else if (stopwatch.IsRunning)
            {
                btnLapReset.Text = "Lap";
                timingID++;
                string time = timingID.ToString() + " : " + stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
                DisplayTimings.Add(new DisplayTiming { Duration = time });
                ListOfTimings.Add(FactoryOfTiming(time));
            }
        }
        async void BtnSave_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Save", "Would you like to Save?", "Yes", "No");
            if (answer == false)
            {
                return;
            }
            else if (answer == true)
            {
                if (ListOfTimings.Count <= 0) //check for any timings
                {
                    await App.Database.DeleteResultAsync(result);
                    await Navigation.PopAsync();
                }
                else 
                {
                    string resultname = await DisplayPromptAsync("Company & Type of Activity", "", placeholder: "Company & Activity"); // All the goddamn checks for inputs
                    if(resultname == null || resultname == "") 
                    { 
                        await DisplayAlert("Missing Name of Conduct", "Please enter a name", "OK");
                        return;
                    }
                    string ConName = await DisplayPromptAsync("Conducting Officer", "", placeholder: "Name");
                    if (ConName == null || ConName == "")
                    {
                        await DisplayAlert("Missing The name of Conducting officer", "Please enter a name", "OK");
                        return;
                    }
                    string SupName = await DisplayPromptAsync("Supervising Officer", "", placeholder: "Name");
                    if (SupName == null || SupName == "")
                    {
                        await DisplayAlert("Missing The name of Supervising officer", "Please enter a name", "OK");
                        return;
                    }
                    string NeuName = await DisplayPromptAsync("Neutral Officer", "", placeholder: "Name");
                    if (NeuName == null || NeuName == "")
                    {
                        await DisplayAlert("Missing The name of Neutral officer", "Please enter a name", "OK");
                        return;
                    }
                    if (resultname == null || resultname == "" || ConName == null || ConName == "" || SupName == null || SupName == "" || NeuName == null || NeuName == "")
                    {
                        await DisplayAlert("An unexpected error occured", "Please try saving again", "OK");
                    }
                    else
                    {
                        for (int i = 0; i < ListOfTimings.Count; i++)
                        {
                            await App.Database.SaveTimingAsync(ListOfTimings[i]);
                        }

                        var test = result;
                        result.DateCreated = DateTime.Now;
                        result.Name = resultname;
                        result.ConductingName = ConName;
                        result.SupervisingName = SupName;
                        result.NeutralName = NeuName;

                        await App.Database.SaveResultAsync(result);
                        result.Timings = ListOfTimings;
                        await App.Database.PopulateResultTimingList(result);

                        await Navigation.PushModalAsync(new Signature { BindingContext = result as Result });
                        Navigation.InsertPageBefore(new ResultViewPage { BindingContext = result as Result }, this);
                        await Navigation.PopAsync();
                    }
                }
            }
        }
    }
}