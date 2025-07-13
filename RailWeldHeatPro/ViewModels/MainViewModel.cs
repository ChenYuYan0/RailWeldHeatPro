using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using RailWeldHeatPro.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly MachineToolView _machineToolView;
        private readonly GlobalConfig _globalConfig;
        private readonly ParamsView _paramsView;
        private readonly ChartView _chartView;
        private readonly DeviceView _deviceView;
        private readonly LogView _logView;

        
        /// <summary>
        /// 左侧边栏ListView存放的目录列表
        /// </summary>
        [ObservableProperty]
        private MainWindowItem[] _demoItems;

        /// <summary>
        /// 软件版本号
        /// </summary>
        [ObservableProperty]
        private double _versionNumber;

        /// <summary>
        /// plc通信状态
        /// </summary>
        [ObservableProperty]
        private bool _connectionState;

        /// <summary>
        /// 系统时间
        /// </summary>
        [ObservableProperty]
        private string _systempDateTime;

        /// <summary>
        /// 通讯延时
        /// </summary>
        [ObservableProperty]
        private string _connectionDelay;

        /// <summary>
        /// 焊缝编号
        /// </summary>
        [ObservableProperty]
        private string _productNumber;


        public MainViewModel(LoginViewModel loginViewModel,MachineToolView machineToolView, GlobalConfig globalConfig,ParamsView paramsView,ChartView chartView,DeviceView deviceView,LogView logView) {
            //依赖注入
            _loginViewModel = loginViewModel;
            _machineToolView = machineToolView;
            _globalConfig = globalConfig;
            _paramsView = paramsView;
            _chartView = chartView;
            _deviceView = deviceView;
            _logView = logView;


            // 订阅plc连接状态变化事件
            _globalConfig.ConnectionStateChanged += OnConnectionStateChanged;
            InitMainViewDemoItems();
            InitFixedParam();
            InitDataBase();

        }
        /// <summary>
        /// 初始化数据库codeFirst
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitDataBase()
        {
            if (false)
            {
                //建库
                SqlSugarHelper.Db.DbMaintenance.CreateDatabase();

                //建表
                SqlSugarHelper.Db.CodeFirst.InitTables<PlcDataReadItem>();
            }
        }

        /// <summary>
        /// 更新plc连接状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isConnected"></param>
        private void OnConnectionStateChanged(object sender, bool isConnected)
        {
            // 在UI线程更新状态
            App.Current.Dispatcher.Invoke(() =>
            {
                ConnectionState = isConnected;
            });
        }



        /// <summary>
        /// 初始化固定(只读取一遍)的参数(焊缝编号、版本号)
        /// </summary>
        private void InitFixedParam()
        {
            ProductNumber = _loginViewModel.ProductNumberFull;
            //后续版本后可以考虑存入ini配置文件
            VersionNumber = 1.0;
        }

        /// <summary>
        /// 初始化主菜单侧边栏选项
        /// </summary>
        private void InitMainViewDemoItems()
        {
            DemoItems = new MainWindowItem[] {
                new MainWindowItem("机床信息", _machineToolView )
                , new MainWindowItem("系统参数",_paramsView)
                , new MainWindowItem("实时曲线",_chartView)
                ,new MainWindowItem("控制界面",_deviceView)
                ,new MainWindowItem("日志管理",_logView)
                
            };
        }
    }
}
