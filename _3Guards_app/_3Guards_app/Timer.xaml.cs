using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using _3Guards_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timer : ContentPage
    {   
        readonly Stopwatch stopwatch;
        private int timingID = 0;

        //public ObservableCollection<DisplayTiming> DisplayTimings { get; } = new ObservableCollection<DisplayTiming>();
        ObservableCollection<DisplayTiming> displayTimings = new ObservableCollection<DisplayTiming>();
        public ObservableCollection<DisplayTiming> DisplayTimings { get { return displayTimings; } }
        public class DisplayTiming
        {
            public string Duration { get; set; }
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
            string time = timingID.ToString() + " : " + stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
            DisplayTimings.Add(new DisplayTiming { Duration = time });

            

          
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

           

           // await App.Database.SaveResultAsync(result);
            await Navigation.PopAsync();
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
           
        }
    }
}