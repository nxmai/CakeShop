using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for StatisticsScreen.xaml
    /// </summary>
    /// 
   
    public partial class StatisticsScreen : Window
    {

        cakeshopEntities db = new cakeshopEntities();
        public StatisticsScreen()
        {
            InitializeComponent();
            createColumnChart();
            createPieChart();
        }

       

        private void createColumnChart()
        {
            SeriesCollection columnData = new SeriesCollection();

            int [] revenue= new int[12];
            for (int i =0;i<revenue.Length; i++)
            {
                revenue[i] = 0;
            }

            var tempCart = db.carts.Where(p => p.isCompleted == true).ToList();

            foreach (var temp in tempCart)
            {
                int month = temp.createAt.Value.Month;

                revenue[month-1] += (int) temp.total;
            }

            for (int i= 0; i<revenue.Length; i++)
            {
                columnData.Add(new ColumnSeries
                {
                    Title = "Tháng " + (i+1).ToString(),
                    Values = new ChartValues<float> { revenue[i] },

                });

            }



            columnChart.Series = columnData;
        }


        private void pieClick(object sender, ChartPoint chartPoint)
        {
            var chart = chartPoint.ChartView as PieChart;
            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }
            var selectedSeries = chartPoint.SeriesView as PieSeries;
            selectedSeries.PushOut = 15;

        }

        private void createPieChart()
        {
            var querry = db.cakes.Join(
                         db.cakeInCarts,
                         p => p.cakeId,
                         d => d.cakeId,
                         (p, d) => new
                         {
                             catID = p.categoryId,
                             price = p.price,
                             amount = d.amount,
                             cartid = d.cartId,
                         }).Join(
                db.carts,
                pd => pd.cartid,
                c =>c.cartId,
                (pd,c) => new{
                    catID = pd.catID,
                    price = pd.price,
                    amount = pd.amount,
                    isCompleted = c.isCompleted,
                }
                ).Where(
                pd => pd.isCompleted == true).GroupBy(
                pd => pd.catID,
                (key, listCol) => new
                {
                    catID = key,
                    revenue = listCol.Sum(total => total.price *  total.amount)
                }
                ).Join(db.categories,
                pd => pd.catID,
                cat => cat.catId,
                (pd, cat) => new
                {
                    catID = pd.catID,
                    catName = cat.name,
                    revenue = pd.revenue

                }).ToList();

            SeriesCollection piechartData = new SeriesCollection();

            foreach (var temp  in  querry)
            {
                piechartData.Add(new PieSeries
                {
                    Title = temp.catName,
                    Values = new ChartValues<float> { (int)temp.revenue },

                });

            }

            pieChart.Series = piechartData;
        }
    }
}
