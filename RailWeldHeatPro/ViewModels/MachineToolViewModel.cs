using CommunityToolkit.Mvvm.ComponentModel;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class MachineToolViewModel : ObservableObject
    {
        [ObservableProperty]
        PlcDataReadItem _plcDataReadItemProp = new();

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly GlobalConfig _globalConfig;

        public MachineToolViewModel(GlobalConfig globalConfig)
        {
            _globalConfig = globalConfig;
            //_globalConfig.StartCollectionAsync();
            InitReadDicToPlcItemProp();

        }


        /// <summary>
        /// 将实时字典中的数据导入到_plcDataReadItemProp中
        /// </summary>
        private void InitReadDicToPlcItemProp()
        {
            if (_globalConfig.ReadDataDic == null)
            {
                _globalConfig.ReadDataDic = new ConcurrentDictionary<string, object>();
            }
            Task.Run(async () =>
            {
                var properties = typeof(PlcDataReadItem).GetProperties().Where(p=>p.PropertyType==typeof(float)||p.PropertyType==typeof(bool));
                while (!_cts.IsCancellationRequested)
                {
                    foreach (var property in properties) {

                        try
                        {
                            if (property.PropertyType == typeof(float))
                            {
                                var value = _globalConfig.GetValue<float>(property.Name);
                                
                                property.SetValue(PlcDataReadItemProp, value);
                            }else if(property.PropertyType == typeof(bool))
                            {
                                var value = _globalConfig.GetValue<bool>(property.Name);
                                property.SetValue(PlcDataReadItemProp, value);
                            }
                        }
                        catch (Exception e)
                        {

                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                    
                    }
                    await Task.Delay(1000);

                }
            });
        }

    }
}
