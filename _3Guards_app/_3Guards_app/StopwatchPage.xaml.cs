using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _3Guards_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopwatchPage : ContentPage
    {   
        readonly Stopwatch stopwatch;
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
            stopwatch = new Stopwatch();

            DisplayTimingsView.ItemsSource = displayTimings;

            //Which button should be visible when initialised
            btnStop.IsVisible = false;
            btnSplit.IsVisible = false;
            btnSave.IsVisible = false;
            lblStopwatch.Text = "00:00.00";

        }
        async void OnResultsPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultsPage());

        }

        public void BtnStart_Clicked(object sender, EventArgs e)
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                btnStart.IsVisible = false;
                btnStop.IsVisible = true;
                btnSplit.IsVisible = true;
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
        }

        
        private void BtnSplit_Clicked(Object sender, EventArgs e)
        {
            btnSplit.Text = "Split";
            timingID++;
            string time = timingID.ToString() + " : " + stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
            DisplayTimings.Add(new DisplayTiming { Duration = time });
            ListOfTimings.Add(FactoryOfTiming(time));
        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            //Display Update
            btnStart.IsVisible = true;
            btnStop.IsVisible = false;
            btnSplit.IsVisible = false;
            btnSave.IsVisible = true;
            btnStart.Text = "Resume";
            stopwatch.Stop();
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
        private void BtnReset_Clicked(object sender, EventArgs e)
        {
            // Display Update
            stopwatch.Reset();
            DisplayTimings.Clear();
            btnSave.IsVisible = false;
            btnStart.IsVisible = true;
            btnStop.IsVisible = false;
            btnSplit.IsVisible = false;
            lblStopwatch.Text = "00:00.00";
            btnStart.Text = "Start";

            // DataUpdate
            timingID = 0;

            //rmb delete for debug purpose
            App.Database.Reset();

        }
    }
}