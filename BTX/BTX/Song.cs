using System;
using System.Collections.Generic;
using System.Text;

namespace BTX
{
    public class Song
    {
        public string ChartURL { get; set; }
        public string ChartTitle { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int CurrentRank { get; set; }
        public int PrevRank { get; set; }
        public int PeakPosition { get; set; }
        public int WeeksOnChart { get; set; }

    }
}
