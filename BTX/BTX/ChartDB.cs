using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

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
        public void FillFromURL(String url)
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
                    Charts.Add(NewChart);
                    ChartNumber++;
                }
            }
            ReadStream.Close();
            response.Close();

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
    }
}
