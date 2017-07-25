using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using SQLite;

namespace BTX
{
    public class ChartDB
    {
        private List<Chart> Charts;
        private Chart curSelectedChart;
        private ChartDB()
        {
            Charts = new List<Chart>();
        }
        private static ChartDB _instance;
        public static ChartDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChartDB();
                }
                return _instance;
            }
        }
        public List<Chart> GetCharts()
        {
            return Charts;
        }
        public Chart GetChartByNumber(int ChartNumber)
        {
            Chart selChart = null;
            foreach(Chart curChart in Charts)
            {
                if (curChart.ChartNumber == ChartNumber)
                {
                    selChart = curChart;
                    break;
                }
            }
            return selChart;
        }
        public void FillFromURL(String url)
        {
            var db = new SQLiteConnection (Config.Instance.dbPath);
            if (db.Table<Chart>().Count() == 0)
            {
                var myBuilder = new UriBuilder(url);
                myBuilder.Path = String.Empty;
                var baseUri = myBuilder.Uri;
                var baseUrl = baseUri.ToString();

                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                //// Send the request to the server and wait for the response:
                WebResponse response = request.GetResponse();
                Stream ReceiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader ReadStream = new StreamReader(ReceiveStream, encode);

                String Line;
                String ChartTag = "<a href=\"";
                int ChartNumber = 1;
                int ChartTagLen = ChartTag.Length;
                int ChartTagStart = 0;
                int ChartTagEnd = 0;
                String ChartURL;
                String ChartTitle;

                // Read the stream to a string, and write the string to the console.
                while (ReadStream.Peek() > 0)
                {
                    Line = ReadStream.ReadLine();
                    if (Line.IndexOf("<span class=\"field-content\">") > 0)
                    {
                        ChartTagStart = Line.IndexOf(ChartTag);
                        ChartTagEnd = Line.IndexOf("\">", ChartTagStart);
                        ChartURL = baseUrl + Line.Substring(ChartTagStart + ChartTagLen + 1, ChartTagEnd - ChartTagStart - ChartTagLen - 1);
                        ChartTitle = Line.Substring(ChartTagEnd + 2, Line.IndexOf("</a>") - ChartTagEnd - 2);
                        //Console.WriteLine(chartURL + ":" + chartTitle);
                        Chart NewChart = new Chart();
                        NewChart.ChartNumber = ChartNumber;
                        NewChart.ChartNumberLabel = ChartNumber.ToString();
                        NewChart.ChartURL = ChartURL;
                        NewChart.ChartTitle = WebUtility.HtmlDecode(ChartTitle);
                        NewChart.Favorite = 0;
                        NewChart.Hide = 0;
                        Charts.Add(NewChart);
                        db.Insert (NewChart);
                        ChartNumber++;
                    }
                }
                ReadStream.Close();
                response.Close();
            }
            else
            {
                var query = db.Table<Chart>().Where (v => v.Hide == 0);
                foreach (var c in query) {
                    Charts.Add(c);
                }
            }
            db.Close();
        }
        public void SetCurSelectedChart(int ChartNumber)
        {
            foreach (Chart curChart in Charts)
            {
                if (curChart.ChartNumber == ChartNumber)
                {
                    curSelectedChart = curChart;
                    break;
                }
            }
            
        }
        public Chart GetCurSelectedChart()
        {
            return curSelectedChart;
        }

        public void ReloadAll()
        {
            Charts.Clear();
            var db = new SQLiteConnection (Config.Instance.dbPath);
            var table = db.Table<Chart> ();
            foreach (var c in table) {
                Charts.Add(c);
            }
            db.Close();
        }

        public void ReloadFavsOnly()
        {
            Charts.Clear();
            var db = new SQLiteConnection (Config.Instance.dbPath);
            var query = db.Table<Chart>().Where (v => v.Favorite == 1);
            foreach (var c in query) {
                Charts.Add(c);
            }
            db.Close();
        }
        public void ReloadVis()
        {
            Charts.Clear();
            var db = new SQLiteConnection (Config.Instance.dbPath);
            var query = db.Table<Chart>().Where (v => v.Hide == 0);
            foreach (var c in query) {
                Charts.Add(c);
            }
            db.Close();
        }

    }
}
