using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.IO;

namespace BTX
{
	public partial class App : Application
	{
        // dbPath contains a valid file path for the database file to be stored
        public App()
        {
            InitializeComponent();
            Config.Instance.dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "BTXdb.db3");
            var db = new SQLiteConnection(Config.Instance.dbPath);
            db.CreateTable<Chart>();
            db.Close();
            ChartDB.Instance.FillFromURL("http://www.billboard.com/charts");
            //MainPage = new BTX.MainPage();
			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ChartsPage())
                    {
                        Title = "Charts"
                    },
                    new NavigationPage(new SongsPage())
                    {
                        Title = "Songs"
                    },
                }
            };
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
