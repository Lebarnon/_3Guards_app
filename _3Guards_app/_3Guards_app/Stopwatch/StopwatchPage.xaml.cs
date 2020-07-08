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
        readonly System.Diagnostics.Stopwatch stopwatch;
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
            stopwatch = new System.Diagnostics.Stopwatch();

            //only start button
            btnLapReset.IsVisible = false;
            btnStartStop.SetValue(Grid.ColumnSpanProperty, 2);

            DisplayTimingsView.ItemsSource = displayTimings;

            lblStopwatch.Text = "00:00.00";
        }
        async void OnResultsPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultsPage());

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
                btnStartStop.BackgroundColor = Color.FromHex("#a60000");

                btnLapReset.Text = "Lap";
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
            }
            else if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                btnStartStop.Text = "Resume";
                btnStartStop.BackgroundColor = Color.White;

                btnLapReset.Text = "Reset";
                btnLapReset.BackgroundColor = Color.FromHex("#a60000");
                btnSave.IsVisible = true;
            }
        }

        
        private void BtnLapReset_Clicked(Object sender, EventArgs e)
        {
            if (!stopwatch.IsRunning)
            {
                // Display Update
                stopwatch.Reset();
                DisplayTimings.Clear();

                btnLapReset.IsVisible = false;
                btnStartStop.SetValue(Grid.ColumnSpanProperty, 2);
                btnStartStop.BackgroundColor = Color.White;
                btnStartStop.Text = "Start";

                lblStopwatch.Text = "00:00.00";
               
                btnSave.IsVisible = false;

                //rmb delete for debug purpose
                App.Database.Reset();
                // DataUpdate
                timingID = 0;

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
            
            for (int i = 0; i < ListOfTimings.Count; i++)
            {
                await App.Database.SaveTimingAsync(ListOfTimings[i]);
            }

            if (ListOfTimings.Count <= 0)
            {
                await App.Database.DeleteResultAsync(result);
                await Navigation.PopAsync();
            }
            else
            {
                string resultname = await DisplayPromptAsync("Company Involved", "" ,placeholder: "Company");
                var test = result;
                result.DateCreated = DateTime.Now;
                result.Name = resultname;
                await App.Database.SaveResultAsync(result);
                result.Timings = ListOfTimings;
                await App.Database.PopulateResultTimingList(result);
                
                await Navigation.PushAsync(new Signature { BindingContext = result as Result });
                Navigation.RemovePage(this);
            }

        }
    }
}