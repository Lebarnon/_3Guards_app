using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using PdfSharp.Xamarin.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _3Guards_app.Models;
using static Xamarin.Essentials.Permissions;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharpCore.Drawing.Layout;

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

            Conducting.Source = GetFromDisk(result.ConductingSig);
            Supervising.Source = GetFromDisk(result.SupervisingSig);
            Safety.Source = GetFromDisk(result.SafetySig);

        }
        private static ImageSource GetFromDisk(string imageFileName)
        {
            var imageAsBase64String = Xamarin.Essentials.Preferences.Get(imageFileName, string.Empty);

            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageAsBase64String)));
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var result = (Result)BindingContext;
            await App.Database.DeleteResultAsync(result);
            await Navigation.PopAsync();

        }
        // generating PDF
        private async void GeneratePDF(object sender, EventArgs e)
        {
            var result = (Result)BindingContext;
            List<Timing> timings = await App.Database.GetTimingsAsync(result.ID);

            await GetReadWriteStoragePermission();

            PdfDocument _document = new PdfDocument();
            PdfPage page = _document.AddPage();

            //XFont font = new XFont("Times", 25, XFontStyle.Bold);
            //page.Size = PageSize.A4;
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            //gfx.DrawString(timings[1].Time, font, XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopLeft);

            //test2//
            const string text =
              "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
              "Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
              "facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
              "lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
              "aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
              "Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
              "henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
              "dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
              "Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
              "dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
              "Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
              "min ut in ute doloboreet ip ex et am dunt at.";

            //IF ABOVE WORKS TRY THIS BELOW

            string test = "";

            foreach (Timing time in timings)
            {
                test += time.Time;
                test += Environment.NewLine;
            }

            XGraphics gfx = XGraphics.FromPdfPage(page);
            
            XFont font = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect = new XRect(20, 100, 250, 220); //play around with this
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            //tf.Alignment = ParagraphAlignment.Left;
            tf.DrawString(test, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            
           

            //SAVING
            string fileName = result.Name + ".pdf";
            // add some checks if file exist
            DependencyService.Get<IPdfSave>().Save(_document, fileName);
           
            Device.OpenUri()
            //Process.Start(fileName); //try opening saved file.
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
    }
}   