using Microsoft.Extensions.DependencyInjection;
using RailWeldHeatPro.ViewModels;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace RailWeldHeatPro.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            MainViewModel viewModel = App.Current.ServiceProviders.GetService<MainViewModel>();
            DataContext = viewModel;
        }

        private void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Content)
            {
                case "重启计算机":
                    //if (CMessageBoxResult.Yes == CMessageBox.Show("确定重启计算机吗？", "提示", CMessageBoxButton.YesNO, CMessageBoxImage.Question))
                    //{
                    //    //点击了显示一个提示的窗体
                    //    this.TextBlock_DialogMessageConent.Text = "正在退出，请稍后...";
                    //    DialogHostWait.IsOpen = true;
                    //    await Task.Delay(1000);
                    //    DialogHostWait.IsOpen = false;
                    //    this.Close();
                    //    System.Environment.Exit(0);
                    //    Process.Start("shutdown.exe", "-r");//重启
                    //}
                    Console.WriteLine("aaa");
                    break;
                case "关闭计算机":
                    //if (CMessageBoxResult.Yes == CMessageBox.Show("确定关闭计算机吗？", "提示", CMessageBoxButton.YesNO, CMessageBoxImage.Question))
                    //{
                    //    //点击了显示一个提示的窗体
                    //    this.TextBlock_DialogMessageConent.Text = "正在退出，请稍后...";
                    //    DialogHostWait.IsOpen = true;
                    //    await Task.Delay(1000);
                    //    DialogHostWait.IsOpen = false;
                    //    this.Close();
                    //    System.Environment.Exit(0);
                    //    Process.Start("shutdown.exe", "-s");//关机
                    //}
                    Console.WriteLine("aaa");
                    break;
                case "退出系统":
                //if (CMessageBoxResult.Yes == CMessageBox.Show("确定退出系统吗？", "提示", CMessageBoxButton.YesNO, CMessageBoxImage.Question))
                //{
                //    //点击了显示一个提示的窗体
                //    this.TextBlock_DialogMessageConent.Text = "正在退出，请稍后...";
                //    DialogHostWait.IsOpen = true;
                //    await Task.Delay(1000);
                //    DialogHostWait.IsOpen = false;
                //    this.Close();
                //    System.Environment.Exit(0);
                //}
                Console.WriteLine("aaa");
                break;

            }
        }

        /// <summary>
        /// 点击菜单选项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

    }


}
