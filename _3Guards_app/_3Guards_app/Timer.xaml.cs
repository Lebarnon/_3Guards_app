using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _3Guards_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;


namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timer : ContentPage
    {   
        readonly Stopwatch stopwatch;
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
        public static Timing FactoryOfTiming(int id, string timing)
        {
            return new Timing() {
                ID = id,
                Time = timing
            };
        }

        public Timer()
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
        async void Outoftheway(object sender, EventArgs e)
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
            string time = stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
            DisplayTimings.Add(new DisplayTiming { Duration = timingID.ToString() + " : " + time });
            ListOfTimings.Add(FactoryOfTiming(timingID, time));
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

            var list = await App.Database.GetTimingsAsync();
            var list1 = await App.Database.GetTimingAsync(1);
            var list2 = await App.Database.GetTimingAsync(2);
            await Navigation.PushAsync(new ResultsPage());
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