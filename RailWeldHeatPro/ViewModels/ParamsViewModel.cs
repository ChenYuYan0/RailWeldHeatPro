using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class ParamsViewModel : ObservableObject
    {
        /// <summary>
        /// 选中模拟数据按钮
        /// </summary>
        [ObservableProperty]
        private bool _mockCheck ;

        /// <summary>
        /// 选中采集数据按钮
        /// </summary>
        [ObservableProperty]
        private bool _collectCheck;

        private CancellationTokenSource _ctsMock = new();

        private readonly GlobalConfig _globalConfig;

        [ObservableProperty]
        RootParam _rootParamProp;

        public ParamsViewModel(GlobalConfig globalConfig,IOptionsSnapshot<RootParam> optionsSnapshot)
        {
            _globalConfig = globalConfig;
            RootParamProp = optionsSnapshot.Value;
        }

        /// <summary>
        /// 切换采集数据命令
        /// </summary>
        [RelayCommand]
        void CollectData()
        {
            if(CollectCheck)
            {
                _globalConfig.StartCollectionAsync();
                _globalConfig.InitQueue();
            }
            else
            {
                _globalConfig._startColCts.Cancel();
                _globalConfig._startColCts.Dispose();
                _globalConfig._saveToDbCts.Cancel();
                _globalConfig._saveToDbCts.Dispose();
            }
        }



        /// <summary>
        /// 切换模拟数据按钮命令
        /// </summary>
        [RelayCommand]
        void MockData()
        {
            if (MockCheck)
            {
                StartMock();
            }
            else
            {
                StopMock();
            }
        }

        /// <summary>
        /// 开始模拟数据 思路：从PlcDataReadItem中获得所有float属性，然后通过global类赋值
        /// </summary>
        private void StartMock()
        {
            _ctsMock = new CancellationTokenSource();


            Task.Run(async () =>
            {

                var floatProperties = typeof(PlcDataReadItem).GetProperties().Where(p => p.PropertyType == typeof(float));


                while (!_ctsMock.Token.IsCancellationRequested)
                {
                    var random = new Random();
                    //拿到每个属性的地址(利用名称相等) 和 值
                    foreach (var floatPropertie in floatProperties)
                    {
                        var value = (float)(random.NextDouble() * 4 + 1);
                        //找出excel数据中名称于floatPropertie的Name相同的第一个元素的地址
                        var address = _globalConfig.ReadEntities.FirstOrDefault(p => p.Name == floatPropertie.Name)?.Address;

                        if (address != null)
                        {
                            await _globalConfig.Plc.WriteAsync(address, value);
                        }
                    }
                    await Task.Delay(5000);
                }
            }, _ctsMock.Token);

        }

        /// <summary>
        /// 停止模拟数据
        /// </summary>
        private void StopMock()
        {
            _ctsMock.Cancel();
            _ctsMock.Dispose();
        }
    }
}
