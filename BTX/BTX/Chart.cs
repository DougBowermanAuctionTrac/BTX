using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace BTX
{
    [Table("Chart")]
    public class Chart
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ChartNumber { get; set; }
        [MaxLength(128)]
        public string ChartNumberLabel { get; set; }
        [MaxLength(256)]
        public string ChartURL { get; set; }
        [MaxLength(128)]
        public string ChartTitle { get; set; }
        public int Favorite { get; set; }
        public int Hide { get; set; }
    }
}
