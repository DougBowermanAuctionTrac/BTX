using System;
using System.Collections.Generic;
using System.Text;
using BTX;
using System.Windows.Input;

namespace  BTX
{
    public class ChartsViewCellModel
    {
        private Chart ChartObject;


        public ChartsViewCellModel(Chart theChart)
        {
            ChartObject = theChart;
        }

        public string ChartNumberLabel => ChartObject.ChartNumberLabel;
        public string ChartTitle => ChartObject.ChartTitle;
        public string ChartUTL => ChartObject.ChartURL;



    }

}
