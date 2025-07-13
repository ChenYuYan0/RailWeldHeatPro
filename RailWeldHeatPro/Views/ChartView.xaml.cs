using Microsoft.Extensions.DependencyInjection;
using RailWeldHeatPro.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RailWeldHeatPro.Views
{
    /// <summary>
    /// ChartView.xaml 的交互逻辑
    /// </summary>
    public partial class ChartView : UserControl
    {
        private ChartViewModel _chartViewModel;

        public ChartView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            _chartViewModel = App.Current.ServiceProviders.GetService<ChartViewModel>();
            DataContext = _chartViewModel;
            Loaded += ChartView_Loaded;
        }


        private void ChartView_Loaded(object sender, RoutedEventArgs e)
        {
            _chartViewModel.InitPlot(TimeDataPlot);
        }
    }
}
