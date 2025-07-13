using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NLog;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class DeviceViewModel : ObservableObject
    {

        public readonly GlobalConfig GlobalConfigProp;
        private readonly ILogger<ChartViewModel> _logger;

        [ObservableProperty]
        PlcDataReadItem _plcDataReadItem = new();

        [ObservableProperty]
        string _logContent = string.Empty;

        public DeviceViewModel(GlobalConfig globalConfig,ILogger<ChartViewModel> logger)
        {
            GlobalConfigProp = globalConfig;
            _logger = logger;

            LogContent = $"程序运行中...{Environment.NewLine}程序已启动...{Environment.NewLine}";
        }

        //清除日志命令
        [RelayCommand]
        void ClearLog()
        {
            LogContent = string.Empty;
        }


        //写入设备控制命令
        [RelayCommand]
        void WriteDeviceControl(string paramName)
        {
            if (!GlobalConfigProp.PlcState)
            {
                LogContent += "PLC未连接或连接异常" + Environment.NewLine;
                _logger.LogError("PLC未连接或连接异常");
                System.Windows.Forms.MessageBox.Show("PLC未连接或连接异常");

                return;
            }

            var readAddress = GlobalConfigProp.ReadEntities
                .FirstOrDefault(x => x.Name == paramName)?.Address;

            if (string.IsNullOrEmpty(readAddress))
            {
                LogContent += "找不到对应的读取地址" + Environment.NewLine;
                _logger.LogError("找不到对应的读取地址");
                System.Windows.Forms.MessageBox.Show("找不到地址");
                return;
            }

            // 写入 PLC 数据
            var result = GlobalConfigProp.Plc.Write(readAddress, true);
            if (result.IsSuccess)
            {
                // 记录日志
                LogContent += $"写入{paramName} 地址{readAddress} 写入值:true{Environment.NewLine}";
                _logger.LogInformation($"写入{paramName} 地址{readAddress} 写入值:true{Environment.NewLine}");
            }
        }

        //检查是否可以切换采集状态的方法
        private bool CanToggleCollection()
        {
            return GlobalConfigProp.PlcState;
        }

        //切换采集状态命令
        [RelayCommand(CanExecute = nameof(CanToggleCollection))]
        void ToggleCollection(string paramName)
        {
            if (!GlobalConfigProp.PlcState)
            {
                LogContent += "PLC未连接或连接异常" + Environment.NewLine;
                _logger.LogError("PLC未连接或连接异常");
                System.Windows.Forms.MessageBox.Show("plc连接异常");
                return;
            }

            var readAddress = GlobalConfigProp.ReadEntities
                .FirstOrDefault(x => x.Name == paramName);

            if (readAddress == null)
            {
                LogContent += "找不到对应的读取地址" + Environment.NewLine;
                _logger.LogError("找不到对应的读取地址");
                System.Windows.Forms.MessageBox.Show("找不到地址");
                return;
            }

            // 写入 PLC 数据，PlcDataReadItem是当前对象实例_plcDataReadItem
            //通过属性名动态获取对象的属性值,readAddress.En=TemperatureSensor等价于var value = (bool)PlcDataReadItem.TemperatureSensor;
            var value = (bool)PlcDataReadItem.GetType()
                .GetProperty(readAddress.Name)?.GetValue(PlcDataReadItem);

            var result = GlobalConfigProp.Plc.Write(readAddress.Address, value);

            if (result.IsSuccess)
            {
                // 记录日志
                LogContent += $"写入{paramName} 地址{readAddress.Address} 写入值:{value}{Environment.NewLine}";
                _logger.LogInformation($"写入{paramName} 地址{readAddress} 写入值:true{Environment.NewLine}");
            }
        }
    }
}
