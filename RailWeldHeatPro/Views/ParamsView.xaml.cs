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
    /// ParamsView.xaml 的交互逻辑
    /// </summary>
    public partial class ParamsView : UserControl
    {
        public ParamsView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            ParamsViewModel viewModel = App.Current.ServiceProviders.GetService<ParamsViewModel>();
            DataContext = viewModel;
        }
    }
}
