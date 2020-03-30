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

        Result result = new Result();

       

        ObservableCollection<Timing> timings = new ObservableCollection<Timing>();
        public ObservableCollection<Timing> Timings { get { return timings; } }
        
        
        public class Timing
        {
            public string DisplayTiming { get; set; }
        }

        public Timer()
        {
            InitializeComponent();
            stopwatch = new Stopwatch();



            

            TimingView.ItemsSource = timings;

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
            timings.Add(new Timing { DisplayTiming = time });

            
            result.ResultList.Add(time);
        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            btnStart.IsVisible = true;
            btnStop.IsVisible = false;
            btnSplit.IsVisible = false;
            btnSave.IsVisible = true;
            btnStart.Text = "Resume";
            stopwatch.Stop();
        }


        async void BtnSave_Clicked(object sender, EventArgs e)
        {

            result.Date = DateTime.Now;

            await App.Database.SaveResultAsync(result);
            await Navigation.PopAsync();
        }
        private void BtnReset_Clicked(object sender, EventArgs e)
        {
            lblStopwatch.Text = "00:00.00";
            btnStart.Text = "Start";
            stopwatch.Reset();
            timings.Clear();
            btnSave.IsVisible = false;
            btnStart.IsVisible = true;
            btnStop.IsVisible = false;
            btnSplit.IsVisible = false;
            timingID = 0;
            App.Database.DeleteResultAsync(result);
        }
    }
}