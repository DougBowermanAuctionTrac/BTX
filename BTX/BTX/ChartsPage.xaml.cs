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
	public partial class ChartsPage : ContentPage
	{
        ObservableCollection<Chart> myCharts;
        public ChartsPage()
        {
            InitializeComponent();

            //items = new ObservableCollection<string> { "alpha", "beta", "gamma", "delta", "epsilon" };
            myCharts = new ObservableCollection<Chart>();
            foreach (Chart myChart in ChartDB.Instance.GetCharts())
            {
                myCharts.Add(myChart);
            }
            listView.ItemsSource = myCharts;
            //listView.SetBinding(, new Binding("ChartNumber"));
            //listView.SetBinding(Label.TextProperty, new Binding("ChartTitle"));
            //listView.BindingContext = myCharts;
        }
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return; // has been set to null, do not 'process' tapped event
            //DisplayAlert("Tapped", e.SelectedItem + " row was tapped", "OK");
            Chart selChart = (Chart)e.SelectedItem;
            ChartDB.Instance.SetCurSelectedChart(selChart.ChartNumber);
            SongDB.Instance.FillFromURL(selChart.ChartURL);
            MessagingCenter.Send<ChartsPage>(this, "ChartSelected");
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
}