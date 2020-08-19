using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using static Xamarin.Essentials.Permissions;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharpCore.Drawing.Layout;
using _3Guards_app.Stopwatch;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats;

namespace _3Guards_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultViewPage : ContentPage
    {
        public ResultViewPage()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var result = (Result)BindingContext;
            var timings = await App.Database.GetTimingsAsync(result.ID);

            listView.ItemsSource = timings;

            //Conducting.Source = GetFromDisk(result.ConductingSig);
            //Supervising.Source = GetFromDisk(result.SupervisingSig);
            //Safety.Source = GetFromDisk(result.SafetySig);

        }
        async void OnSignatureViewClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new SignatureViewPage
            {
                BindingContext = (Result)BindingContext as Result
            }) ;
            
        }


        private async void GeneratePDF(object sender, EventArgs e) // Saving as PDF
        {
            var result = (Result)BindingContext;
            List<Timing> timings = await App.Database.GetTimingsAsync(result.ID);
            //checking permissions
            await GetReadWriteStoragePermission();
            
            PdfDocument _document = new PdfDocument();
            PdfPage page = _document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            //var test = page.Height; //842
            //var test2 = page.Width; //595
            DrawTimings(gfx, timings);
            DrawSignature(gfx, result.ConductingSig, 2);
            DrawSignature(gfx, result.SupervisingSig, 1);
            DrawSignature(gfx, result.NeutralSig, 3);
            DrawSigVisual(gfx, result.SupervisingName, result.ConductingName, result.NeutralName);
            DrawTitle(gfx, result.Name, result.DateCreated.ToString());
            SaveAndOpen(_document, result, true);
        }
        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete", "Result will be permenantly deleted", "Confirm", "Cancel");
            if (answer == false)
            {
                return;
            }
            else if (answer == true)
            {
                var result = (Result)BindingContext;
                await App.Database.DeleteResultAsync(result);
                await Navigation.PopAsync();
            }
               

        }
        public async Task GetReadWriteStoragePermission()
        {
            var status = await CheckAndRequestPermissionAsync(new ReadWriteStoragePermission());
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission", "Permission to access your mobile storage is required to Save Timings", "OK");
                await RequestAsync<ReadWriteStoragePermission>();
                return;
            }

            
        }
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }
        //PDF Methods
        #region
        private void SaveAndOpen(PdfDocument document, Result result, bool willOpen)
        {
            string fileName = result.Name + ".pdf";

            // add some checks if file exist (future)
            DependencyService.Get<IPdfSave>().Save(document, fileName);
            if(willOpen != true)
            {
                return;
            }
            else
            {
                DependencyService.Get<IPdfOpen>().Open(fileName);
            }
           
        }
        private void DrawTimings(XGraphics gfx, List<Timing> timings)
        {
            //GlobalFontSettings.FontResolver = new FileFontResolver();
            XFont font = new XFont("sans-serif", 10, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect = new XRect(60, 100, 75, 550);
            // Finding number of Rows needed
            int numOfRow = 1;  
            for (int i = timings.Count(); i > 40; numOfRow++)
            {
                i -= 40;
            }

            // populating the rows in PDF
            int lowerLimit = -40;
            int upperLimit = 0;
            for (int i = 0; i < numOfRow; i++)
            {
                lowerLimit += 40;
                upperLimit += 40;
                string temp = "";
                if (upperLimit > timings.Count())
                {
                    upperLimit = timings.Count();
                }
                for (int z = lowerLimit; z < upperLimit; z++)
                {
                    temp += timings[z].Time;
                    temp += Environment.NewLine;

                }

                rect = new XRect(60 + i * 75, 100, 75, 550);
                //gfx.DrawRectangle(XBrushes.Blue, rect);
                tf.DrawString(temp, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }
        }
        private void DrawSignature(XGraphics gfx, string imageFileName, int SigType)
        {
            var imageAsBase64String = Preferences.Get(imageFileName, string.Empty);

            XImage image = XImage.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
            XPoint point = new XPoint(100, 700);
            
            if (SigType == 1)
            {
                gfx.DrawImage(image, 60 ,700, 60, 60);
            }
            else if (SigType == 2)
            {
                gfx.DrawImage(image, 267, 700, 60, 60);
            }
            else if (SigType == 3)
            {
                gfx.DrawImage(image, 475, 700, 60, 60);
            }
        }
       
        public void DrawTitle(XGraphics gfx, string title, string datetime)
        {
            XRect rect = new XRect(new XPoint(), gfx.PageSize);
            rect.Inflate(-10, -15);
            XFont font = new XFont("sans-serif", 16, XFontStyle.Bold);
            gfx.DrawString(title, font, XBrushes.Black, rect, XStringFormats.TopCenter);

            rect.Offset(0, 35);
            font = new XFont("sans-serif", 13, XFontStyle.Italic);
            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Far;

            string newdatetime = datetime.Remove(datetime.Length - 3);
            string recordDate = "Recorded on : " + newdatetime + ".";
            gfx.DrawString(recordDate, font, XBrushes.Black, rect, XStringFormats.TopCenter);
        }

        public void DrawSigVisual(XGraphics gfx, string supN ,string conN, string neuN)
        {
            XFont font = new XFont("sans-serif", 12, XFontStyle.Bold);
            string line = "____________________";
            string sup = "Supervising";
            string con = "Conducting";
            string neu = "Neutral";

            int linepos = 755;
            int rolespos = 770;
            int Namepos = 780;
            //lines
            gfx.DrawString(line, font, XBrushes.Black, new XRect(55, linepos, 70, 10), XStringFormats.TopCenter);
            gfx.DrawString(line, font, XBrushes.Black, new XRect(262, linepos, 70, 10), XStringFormats.TopCenter);
            gfx.DrawString(line, font, XBrushes.Black, new XRect(470, linepos, 70, 10), XStringFormats.TopCenter);
            //roles
            gfx.DrawString(sup, font, XBrushes.Black, new XRect(55, rolespos, 70, 40), XStringFormats.TopCenter);
            gfx.DrawString(con, font, XBrushes.Black, new XRect(262, rolespos, 70, 40), XStringFormats.TopCenter);
            gfx.DrawString(neu, font, XBrushes.Black, new XRect(470, rolespos, 70, 40), XStringFormats.TopCenter);
            //Names
            gfx.DrawString(supN, font, XBrushes.Black, new XRect(55, Namepos, 70, 40), XStringFormats.Center);
            gfx.DrawString(conN, font, XBrushes.Black, new XRect(262, Namepos, 70, 40), XStringFormats.Center);
            gfx.DrawString(neuN, font, XBrushes.Black, new XRect(470, Namepos, 70, 40), XStringFormats.Center);
        }
        #endregion
        //private static ImageSource GetFromDisk(string imageFileName)
        //{
        //    var imageAsBase64String = Preferences.Get(imageFileName, string.Empty);

        //    return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
        //}

       
    }
}   