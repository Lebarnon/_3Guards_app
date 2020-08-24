using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using _3Guards_app.Data;

namespace _3Guards_app
{
    public partial class App : Application
    {
        static GuardsDatabase database = null;
        
        public static GuardsDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new GuardsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "guardsDatabase.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }

        //protected override void OnStart()
        //{

        //}
        //protected override void OnSleep()
        //{

        //}
        //protected override void OnResume()
        //{

        //}
    }
}
