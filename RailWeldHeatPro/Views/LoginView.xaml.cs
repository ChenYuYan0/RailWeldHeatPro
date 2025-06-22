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
using System.Windows.Shapes;

namespace RailWeldHeatPro.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            InitData();
            
        }
        /// <summary>
        /// 设置View的数据上下文为对应ViewModel
        /// </summary>
        private void InitData()
        {
            LoginViewModel viewModel = App.Current.ServiceProviders.GetService<LoginViewModel>();
            DataContext = viewModel;

            //订阅关闭登录窗口事件
            viewModel.CloseLoginView += (s, e) => this.Close();
        }
    }
}
