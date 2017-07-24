using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using BTX;

namespace BTX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongsPage : ContentPage
	{
        ObservableCollection<Song> mySongs;
        //Label CurChartTitleLabel;
        public SongsPage ()
		{
			InitializeComponent ();
            //CurChartTitleLabel = this.FindByName<Label>("ChartTitleLabel");

            MessagingCenter.Subscribe<ChartsPage>(this,"ChartSelected", (sender) => {
                ChangeSongs();
            });
            mySongs = new ObservableCollection<Song>();
            listView.ItemsSource = mySongs;
            //listView.SetBinding(, new Binding("ChartNumber"));
            //listView.SetBinding(Label.TextProperty, new Binding("ChartTitle"));
            //listView.BindingContext = myCharts;
        }
        private void ChangeSongs()
        {
            
            ChartTitleLabel.Text = ChartDB.Instance.GetCurSelectedChart().ChartTitle;

            mySongs.Clear();
            foreach (Song mySong in SongDB.Instance.GetSongs())
            {
                mySongs.Add(mySong);
            }
        }
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            //DisplayAlert("Tapped", e.SelectedItem + " row was tapped", "OK");
            Song selSong = (Song)e.SelectedItem;
            SongDB.Instance.SetCurSelectedSong(selSong.Position);
            MessagingCenter.Send<SongsPage>(this, "SongSelected");
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
}