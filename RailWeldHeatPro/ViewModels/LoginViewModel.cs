using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using RailWeldHeatPro.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace RailWeldHeatPro.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILogger<ChartViewModel> _logger;
        /// <summary>
        /// 操作员名称列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _operatorNames = new ObservableCollection<string>();

        /// <summary>
        /// 选中的操作员名称
        /// </summary>
        [ObservableProperty]
        private string _selectedOperatorName;

        /// <summary>
        /// 生产线名称列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _lineNames = new ObservableCollection<string>();

        /// <summary>
        /// 选中的生产线名称
        /// </summary>
        [ObservableProperty]
        private string _selectedLineName;

        /// <summary>
        /// 班组名称列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _groupNames = new ObservableCollection<string>();

        /// <summary>
        /// 选中的班组名称
        /// </summary>
        [ObservableProperty]
        private string _selectedGroupName;

        //SelectedDate没选中时会将null赋给DateTime，会报错，所以使用DateTime?
        /// <summary>
        /// 选中的日期
        /// </summary>
        [ObservableProperty]
        private DateTime? _selectedDate;

        /// <summary>
        /// 输入的接头号
        /// </summary>
        [ObservableProperty]
        private string _spliceNumber;

        /// <summary>
        /// 完整的焊接编号
        /// </summary>
        [ObservableProperty]
        private string _productNumberFull;

        /// <summary>
        /// 登录窗体关闭的事件
        /// </summary>
        public event EventHandler CloseLoginView;



        public LoginViewModel(ILogger<ChartViewModel> logger)
        {
            _logger = logger;
            InitComboxData();
            
        }

        /// <summary>
        /// 初始化LoginView下拉框选项数据
        /// </summary>
        private void InitComboxData()
        {
            AddDataToComboxItemSource(GlobalPath.OperatorNamesPath, ref _operatorNames);
            AddDataToComboxItemSource(GlobalPath.LineNamesPath, ref _lineNames);
            AddDataToComboxItemSource(GlobalPath.GroupNamesPath, ref _groupNames);
        }

        /// <summary>
        /// 将数据添加到下拉框数据源
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="collection">下拉框选项集合</param>
        private void AddDataToComboxItemSource(String path, ref ObservableCollection<string> collection)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    collection.Add(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        /// <summary>
        /// 登录按钮
        /// </summary>
        [RelayCommand]
        void Login()
        {
           //MessageBox.Show(SelectedOperatorName.ToString()+"-"+SelectedLineName.ToString()+"-" +
           //     SelectedGroupName + "-" + SelectedDate?.ToString("yyyy-MM-dd")+"-"+SpliceNumber);

            //将用户信息序列化成json对象并保存
            LoginUser currentLoginUser = new LoginUser()
            {
                OperatorName = SelectedOperatorName.ToString(),
                LineName = SelectedLineName.ToString(),
                GroupName = SelectedGroupName.ToString(),
                LoginDate = (DateTime)SelectedDate,
                SpliceNumber = SpliceNumber.ToString()
            };
            FileHelper.WriteToFile(GlobalPath.LoginUserPath, JsonConvert.SerializeObject(currentLoginUser));
            //赋值焊接编号
            ProductNumberFull = GetProductNumber(currentLoginUser);

            //先开启主窗体，后关闭登录窗口
            MainView mainView = App.Current.ServiceProviders.GetService<MainView>();
            mainView?.Show();
            _logger.LogInformation("登录系统成功");
            CloseLoginView?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        [RelayCommand]
        void Logout()
        {
            _logger.LogInformation("退出系统成功");
            Environment.Exit(0);//结束进程
        }



        /// <summary>
        /// 获得完整的焊缝编号
        /// </summary>
        /// <param name="loginUser">登录用户对象</param>
        /// <returns>完整焊缝编号</returns>
        public string GetProductNumber(LoginUser loginUser)
        {
            ProductNumberFull = loginUser.OperatorName + loginUser.LineName + loginUser.GroupName + loginUser.LoginDate.ToString("yyyy-MM-dd") + loginUser.SpliceNumber;
            return ProductNumberFull;
        }
    }
}
