using HslCommunication;
using HslCommunication.Profinet.Siemens;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniExcelLibs;
using NLog;
using RailWeldHeatPro.Models;
using RailWeldHeatPro.Ucs;
using RailWeldHeatPro.ViewModels;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RailWeldHeatPro.Helper
{
    public class GlobalConfig : IDisposable
    {
        private readonly IOptionsSnapshot<RootParam> _options;

        private readonly ILogger<GlobalConfig> _logger;
        public bool PlcState { get; set; }

        /// <summary>
        /// 连接plc的客户端
        /// </summary>
        public SiemensS7Net Plc { get; set; }

        /// <summary>
        /// 存放读取excel中所有数据的列表
        /// </summary>
        public List<RailDataReadEntity> ReadEntities { get; set; } = new();

        /// <summary>
        /// 读取的数据字典,(plc变量名称,值)
        /// </summary>
        public ConcurrentDictionary<string, object> ReadDataDic { get; set; }

        /// <summary>
        /// 存放需要保存数据的临时队列
        /// </summary>
        private ConcurrentQueue<PlcDataReadItem> _plcDataReadSaveQueue = new();

        /// <summary>
        /// 心跳检测停止令牌
        /// </summary>
        private CancellationTokenSource _heartbeatCts = new CancellationTokenSource();

        /// <summary>
        /// 采集到ReadDataDic任务停止令牌
        /// </summary>
        public CancellationTokenSource _startColCts = new();

        /// <summary>
        /// 从对列中保存数据到数据库任务的停止令牌
        /// </summary>
        public CancellationTokenSource _saveToDbCts = new();

        //采用事件解决循环依赖问题(原先策略是在GloablConfig中注入MainViewModel，直接invoke修改MainView中的plc状态属性)
        /// <summary>
        /// 连接状态变化事件
        /// </summary>
        public event EventHandler<bool> ConnectionStateChanged;

        public GlobalConfig(IOptionsSnapshot<RootParam> options, ILogger<GlobalConfig> logger)
        {

            _options = options;
            _logger = logger;
            //进入主界面后首先执行一次初始化plc，后不断执行心跳检测Task，后读取excel数据到ReadEntities
            InitPlc();
            PlcHeartTest();
            InitExcelsAddress();
            InitQueue();
        }

        /// <summary>
        /// 向数据库中写入数据
        /// </summary>
        public void InitQueue()
        {
            Task.Run(async () =>
            {
                while (!_saveToDbCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        if(_plcDataReadSaveQueue.TryDequeue(out var data))
                        {
                            //写入数据库
                            await SqlSugarHelper.Db.Insertable(data).ExecuteCommandAsync();
                        }
                        else
                        {
                            await Task.Delay(2000, _saveToDbCts.Token);
                        }
                    }
                    catch (Exception e)
                    {

                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }
            },_saveToDbCts.Token);
            
        }

        /// <summary>
        /// 根据key从实时数据字典从获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">变量名称</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            if (ReadDataDic.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return default;
        }

        /// <summary>
        /// 不断采集数据到ReadDataDic
        /// </summary>
        public Task StartCollectionAsync(CancellationToken? externalToken = null)
        {
            if (ReadDataDic == null)
            {
                ReadDataDic = new ConcurrentDictionary<string, object>();
            }
            //关闭采集任务
            StopCollection();

            //连接内外令牌，有一个取消都将取消整个采集任务
            _startColCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken ?? CancellationToken.None);

            return Task.Run(async () =>
            {
                while (!_startColCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        //OperateResult<byte[]> read = Plc.Read("DB2.0", 141);
                        //按照变量类型赋值给ReadDataDic
                        await UpdatePlcToReadDataDic<float>("DBD");
                        await UpdatePlcToReadDataDic<bool>("DBX");
                        await UpdatePlcToReadDataDic<uint>("DBW");

                        //将需要保存的数据放入临时队列中
                        await SaveQueue();

                        await Task.Delay(1000);


                    }
                    catch (Exception e)
                    {

                        System.Windows.Forms.MessageBox.Show(e.Message);
                    }
                }
            });
        }

        /// <summary>
        /// 将需要保存的数据放入队列中
        /// </summary>
        /// <returns></returns>
        private async Task SaveQueue()
        {
            //需要保存数据的列表
            var saveReadEntities = ReadEntities.Where(x => x.Save==true).ToList();
            if(saveReadEntities.Count < 1)
            {
                return;
            }

            //需要保存的数据对象
            PlcDataReadItem plcDataReadItem = new PlcDataReadItem()
            {
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            foreach (var saveReadEntity in saveReadEntities)
            {
                /**根据读取到的excel中行的名字来反射获取PlcDataReadItem中对应property，
                实现通过excel中的Voltage找到PlcDataReadItem中的Voltage属性，方便后续赋值**/
                var property = typeof(PlcDataReadItem).GetProperty(saveReadEntity.Name);
                if (property != null &&  ReadDataDic.TryGetValue(saveReadEntity.Name,out var value)) {
                    //赋值给plcDataReadItem
                    property.SetValue(plcDataReadItem, value);
                }
                
            }
            //全部float属性赋值完成后将数据添加到对列中
            _plcDataReadSaveQueue.Enqueue(plcDataReadItem);

        }

        /// <summary>
        /// 按照plc变量类型赋值给ReadDataDic
        /// </summary>
        /// <typeparam name="T">变量类型,如float</typeparam>
        /// <param name="addressType">plc地址类型，如DBD</param>
        /// <returns></returns>
        private async Task UpdatePlcToReadDataDic<T>(string addressType)
        {
            try
            {
                //找出所有T类型的ReadEntities
                var addresseReadEntities = ReadEntities.Where(x => x.Address.Contains(addressType)).ToList();
                if (addresseReadEntities.Count < 1)
                {
                    return;
                }

                if (typeof(T) == typeof(bool))
                {
                    foreach (var entity in addresseReadEntities)
                    {

                        var result = await Plc.ReadBoolAsync(entity.Address);
                        if (result.IsSuccess)
                        {
                            ReadDataDic.AddOrUpdate(entity.Name, result.Content, (k, v) => result.Content);
                        }
                        else
                        {
                            // 处理读取失败的情况
                            ReadDataDic.AddOrUpdate(entity.Name, false, (k, v) => false);
                        }
                    }
                }
                else if (typeof(T) == typeof(float))
                {
                    foreach (var entity in addresseReadEntities)
                    {
                        var result = await Plc.ReadFloatAsync(entity.Address);
                        if (result.IsSuccess)
                        {
                            ReadDataDic.AddOrUpdate(entity.Name, result.Content, (k, v) => result.Content);
                        }
                        else
                        {
                            ReadDataDic.AddOrUpdate(entity.Name, 0f, (k, v) => 0f);
                        }

                    }
                }
                else if (typeof(T) == typeof(uint))
                {
                    foreach (var entity in addresseReadEntities)
                    {
                        var result = await Plc.ReadUInt16Async(entity.Address);
                        if (result.IsSuccess)
                        {
                            ReadDataDic.AddOrUpdate(entity.Name, result.Content, (k, v) => result.Content);
                        }
                        else
                        {
                            ReadDataDic.AddOrUpdate(entity.Name, 0u, (k, v) => 0u);
                        }
                    }
                }


            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }



        /// <summary>
        /// 关闭StartCollectionAsync采集任务
        /// </summary>
        private void StopCollection()
        {
            try
            {
                if (_startColCts != null)
                {
                    _startColCts.Cancel();
                    _startColCts.Dispose();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 初始化excel地址并读取数据到ReadEntities
        /// </summary>
        private void InitExcelsAddress()
        {
            try
            {
                ReadEntities = MiniExcel.Query<RailDataReadEntity>(GlobalPath.ReadPath).Where(x => !string.IsNullOrEmpty(x.Address)).ToList();
            }
            catch (Exception)
            {
                _logger.LogError($"找不到配置文件{GlobalPath.ReadPath}");
                System.Windows.Forms.MessageBox.Show("读取文件异常");
            }
        }

        /// <summary>
        /// 初始化plc连接参数
        /// </summary>
        public async Task InitPlc()
        {
            //从json配置文件中获得
            Plc = new SiemensS7Net(SiemensPLCS.S1200, _options.Value.PlcParam.PlcIp);
            Plc.Port = _options.Value.PlcParam.PlcPort;
            Plc.Slot = _options.Value.PlcParam.PlcSlot;
            Plc.Rack = _options.Value.PlcParam.PlcRack;
            Plc.ConnectTimeOut = _options.Value.PlcParam.PlcConnectTimeOut;
            try
            {
                var opResult = await Plc.ConnectServerAsync();
                if (opResult.IsSuccess)
                {
                    PlcState = true;
                    // 触发连接状态变化事件
                    ConnectionStateChanged?.Invoke(this, true);
                }
                else
                {
                    PlcState = false;
                    // 触发连接状态变化事件
                    ConnectionStateChanged?.Invoke(this, false);
                    _logger.LogError($"PLC 连接失败:{_options.Value.PlcParam.PlcIp}:{_options.Value.PlcParam.PlcPort}");
                    System.Windows.MessageBox.Show($"PLC连接失败: {opResult.Message}");
                }
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// plc心跳检测
        /// </summary>
        public async Task PlcHeartTest()
        {
            while (!_heartbeatCts.Token.IsCancellationRequested)
            {
                try
                {
                    // 先检查连接状态
                    if (PlcState == false)
                    {
                        ConnectionStateChanged?.Invoke(this, false);
                        await Task.Delay(1000, _heartbeatCts.Token);
                        continue;
                    }
                    // 心跳检测
                    OperateResult connect = await Plc.ReadBoolAsync("M100");

                    // 触发连接状态变化事件
                    ConnectionStateChanged?.Invoke(this, connect.IsSuccess);
                }
                catch (Exception ex)
                {
                    ConnectionStateChanged?.Invoke(this, false);
                }

                // 添加延时避免CPU占用过高
                await Task.Delay(1000, _heartbeatCts.Token);
            }
        }

        public void Dispose()
        {
            _heartbeatCts.Cancel();
            _heartbeatCts.Dispose();
            Plc?.Dispose();
        }
    }
}
