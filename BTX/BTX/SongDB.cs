using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.IO;

namespace BTX
{
    public class SongDB
    {
        private List<Song> ChartSongs;
        private Song CurSelectedSong;
        private SongDB()
        {
            ChartSongs = new List<Song>();
        }
        private static SongDB _instance;
        public static SongDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SongDB();
                }
                return _instance;
            }
        }
        public void SetCurSelectedSong(int Position)
        {
            foreach (Song curSong in ChartSongs)
            {
                if (curSong.Position == Position)
                {
                    CurSelectedSong = curSong;
                    break;
                }
            }
        }
        public Song GetCurSelectedSong()
        {
            return CurSelectedSong;
        }
        public List<Song> GetSongs()
        {
            return ChartSongs;
        }
        public bool FillFromURL(String url)
        {
            ChartSongs.Clear();
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            //// Send the request to the server and wait for the response:
            WebResponse response = request.GetResponse();
            Stream ReceiveStream = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader ReadStream = new StreamReader(ReceiveStream, encode);

            int SongNum = 0;
            string Line;
            string ChartTitle = "";
            string ChartTitleTag = "<title>";
            string CurrentRank = "";
            string PreviousRank = "";
            string PeakRank = "";
            string WeeksOnChart = "";

            string TitleTag = "data-songtitle=\"";
            string ArtistTag = ">";
            string Title = "";
            string Artist = "";

            bool bGotTitle = false;
            bool bGotArtist = false;
            bool bGotChartRowRank = false;
            bool bGotRowStats = false;


            // Read the stream to a string, and write the string to the console.
            while (ReadStream.Peek() > 0)
            {
                bGotTitle = false;
                bGotArtist = false;
                bGotChartRowRank = false;
                bGotRowStats = false;
                Title = "";
                Artist = "";
                CurrentRank = "0";
                PreviousRank = "0";
                PeakRank = "0";
                WeeksOnChart = "0";

                Line = ReadStream.ReadLine();
                if (Line == null)
                    continue;
                //if (Line.StartsWith(ChartTitleTag))
                //{
                //    ChartTitle = Line.Substring(Line.IndexOf(ChartTitleTag) + ChartTitleTag.Length, Line.Length - (ChartTitleTag.Length * 2) - 1);
                //}
                if (Line.Contains("<article class=\"chart-row"))
                {
                    //Console.WriteLine(Line);
                    int TitlePos = Line.IndexOf(TitleTag) + TitleTag.Length;
                    Title = Line.Substring(TitlePos, Line.IndexOf('"', TitlePos) - TitlePos);
                    bGotTitle = true;
                    SongNum++;
                    while (Line != "</article>")
                    {
                        Line = ReadStream.ReadLine();
                        if (Line == null)
                            break;
                        if (Line.Contains("class=\"chart-row__current-week\""))
                        {
                            CurrentRank = Line.Substring(Line.IndexOf(">") + 1, Line.IndexOf("</") - Line.IndexOf(">") - 1);
                            bGotChartRowRank = true;
                        }
                        if (Line.Contains("class=\"chart-row__artist\""))
                        {
                            // The artist is on the next line, and is the WHOLE line
                            Artist = ReadStream.ReadLine();
                            bGotArtist = true;
                        }
                        if (Line.Contains("class=\"chart-row__stats\""))
                        {
                            bGotRowStats = true;
                        }
                        if ((bGotRowStats) && (Line.Contains("class=\"chart-row__last-week\"")))
                        {
                            Line = ReadStream.ReadLine(); // Label
                            Line = ReadStream.ReadLine(); // Value
                            PreviousRank = Line.Substring(Line.IndexOf(">") + 1, Line.IndexOf("</") - Line.IndexOf(">") - 1);
                        }
                        if ((bGotRowStats) && (Line.Contains("class=\"chart-row__top-spot\"")))
                        {
                            Line = ReadStream.ReadLine(); // Label
                            Line = ReadStream.ReadLine(); // Value
                            PeakRank = Line.Substring(Line.IndexOf(">") + 1, Line.IndexOf("</") - Line.IndexOf(">") - 1);
                        }
                        if ((bGotRowStats) && (Line.Contains("class=\"chart-row__weeks-on-chart\"")))
                        {
                            Line = ReadStream.ReadLine(); // Label
                            Line = ReadStream.ReadLine(); // Value
                            WeeksOnChart = Line.Substring(Line.IndexOf(">") + 1, Line.IndexOf("</") - Line.IndexOf(">") - 1);
                        }

                    }
                    // At end of article then fill song
                    Song mySong = new Song();
                    mySong.Position = SongNum;
                    mySong.CurrentRank = SongNum;
                    mySong.PrevRank = 0;
                    mySong.PeakPosition = 0;
                    mySong.WeeksOnChart = 0;
                    if (bGotTitle)
                    {
                        mySong.Title = WebUtility.HtmlDecode(Title);
                    }
                    if (bGotArtist)
                    {
                        mySong.Artist = WebUtility.HtmlDecode(Artist);
                    }
                    if (bGotChartRowRank)
                    {
                        try
                        {
                            mySong.CurrentRank = Int32.Parse(CurrentRank);
                        }
                        catch (Exception AnyException)
                        {
                            //Console.WriteLine(AnyException.Message);
                        }
                    }
                    if (bGotRowStats)
                    {
                        try
                        {
                            mySong.PrevRank = Int32.Parse(PreviousRank);
                        }
                        catch (Exception AnyException)
                        {
                            //Console.WriteLine(AnyException.Message);
                        }
                        try
                        {
                            mySong.PeakPosition = Int32.Parse(PeakRank);
                        }
                        catch (Exception AnyException)
                        {
                            //Console.WriteLine(AnyException.Message);
                        }
                        try
                        {
                            mySong.WeeksOnChart = Int32.Parse(WeeksOnChart);
                        }
                        catch (Exception AnyException)
                        {
                            //Console.WriteLine(AnyException.Message);
                        }
                    }
                    ChartSongs.Add(mySong);
                }
            }
            return true;
        }
    }
}
