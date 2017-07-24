using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BTX;
using System.Net;

namespace BTX
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongInfoPage : ContentPage
	{
		public SongInfoPage ()
		{
			InitializeComponent ();
            MessagingCenter.Subscribe<SongsPage>(this, "SongSelected", (sender) => {
                ChangeSongInfo();
            });
            //var url = "http://www.google.com";
            //Device.OpenUri(new Uri(url));
        }

        private void ChangeSongInfo()
        {
            Song CurSelectedSong = SongDB.Instance.GetCurSelectedSong();

            TitleLabel.Text = CurSelectedSong.Title;
            ArtistLabel.Text = CurSelectedSong.Artist;
            CurrentRankLabel.Text = CurSelectedSong.CurrentRank.ToString();
            PrevRankLabel.Text = CurSelectedSong.PrevRank.ToString();
            PeakPositionLabel.Text = CurSelectedSong.PeakPosition.ToString();
            WeeksOnChartLabel.Text = CurSelectedSong.WeeksOnChart.ToString();
            String SongArtistTitle;
            SongArtistTitle = CurSelectedSong.Artist + " " + CurSelectedSong.Title;
            SongInfo.Text = "https://play.google.com/music/listen#/sr/" + WebUtility.HtmlEncode(SongArtistTitle.Replace(' ', '+'));
            this.UpdateChildrenLayout();
        }
        private void OnGotoGPMClicked()
        {
            Device.OpenUri(new Uri(SongInfo.Text));
        }
    }
}