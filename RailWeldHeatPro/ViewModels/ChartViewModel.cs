using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NLog;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using ScottPlot;
using ScottPlot.TickGenerators;
using ScottPlot.WPF;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class ChartViewModel : ObservableObject
    {
        //图表控件
        private WpfPlot _timeDataPlot;

        private readonly ILogger<ChartViewModel> _logger;

        [ObservableProperty]
        DateTime _startTime = DateTime.Now.AddDays(-30);

        [ObservableProperty]
        DateTime _endTime = DateTime.Now;

        // 存储已添加的曲线，用于切换显示
        private Dictionary<string, IPlottable> _plots = new Dictionary<string, IPlottable>();

        public ChartViewModel(ILogger<ChartViewModel> logger)
        {
            _logger = logger;
        }

        public void InitPlot(WpfPlot timeDataPlot)
        {
            _timeDataPlot = timeDataPlot;
            ConfigurePlot();
        }

        /// <summary>
        /// 配置图表
        /// </summary>
        private void ConfigurePlot()
        {
            if(_timeDataPlot == null)
            {
                return;
            }
            
        }


        [RelayCommand]
        void Search(string parameter)
        {

            try
            {
                //_timeDataPlot.Plot.Clear();

                //根据创建时间升序排列
                var DbData = SqlSugarHelper.Db.Queryable<PlcDataReadItem>().OrderBy(x => x.CreateTime, SqlSugar.OrderByType.Asc).ToList();

                if (DbData.Count == 0) return;

                // 定义曲线信息（标签、颜色、Y轴标签）
                string label = "";
                System.Drawing.Color color = System.Drawing.Color.Black;
                string yLabel = "";

                switch (parameter)
                {
                    case "Voltage":
                        label = "Voltage";
                        color = System.Drawing.Color.Blue;
                        yLabel = "Voltage(V)";
                        break;
                    case "Current":
                        label = "Current";
                        color = System.Drawing.Color.Red;
                        yLabel = "Current (A)";
                        break;
                    case "Frequency":
                        label = "Frequency";
                        color = System.Drawing.Color.Orange;
                        yLabel = "Frequency (Hz)";
                        break;
                    case "Power":
                        label = "Power";
                        color = System.Drawing.Color.Green;
                        yLabel = "Power (W)";
                        break;
                    case "Energy":
                        label = "Energy";
                        color = System.Drawing.Color.Pink;
                        yLabel = "Energy (J)";
                        break;
                    default:
                        _logger.LogInformation("图像模块传入未知参数");
                        System.Windows.Forms.MessageBox.Show("未知参数: " + parameter);
                        return;
                }
                // 检查曲线是否已存在，避免重复添加
                if (!_plots.ContainsKey(label))
                {
                    AddDataToPlot(DbData, x =>
                    {
                        // 根据参数选择对应的值
                        switch (parameter)
                        {
                            case "Voltage": return x.Voltage;
                            case "Current": return x.Current;
                            case "Frequency": return x.Frequency;
                            case "Power": return x.Power;
                            case "Energy": return x.Energy;
                            default: return 0;
                        }
                    }, label, color);
                }

                // 刷新图表
                _timeDataPlot.Refresh();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 添加数据到图表
        /// </summary>
        private void AddDataToPlot(List<PlcDataReadItem> data, Func<PlcDataReadItem, double> valueSelector, string label, System.Drawing.Color color)
        {
            var values = data.Select(valueSelector).ToArray();
            var signalPlot = _timeDataPlot.Plot.Add.Signal(values);
            signalPlot.LegendText = label;
            signalPlot.Color = new ScottPlot.Color(color.R, color.G, color.B, color.A);
            _plots[label] = signalPlot;
        }
    }
}
