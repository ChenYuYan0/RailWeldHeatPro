using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace RailWeldHeatPro.Models
{
    public class PlcDataReadItem : EntityBase
    {
        #region PLC通讯相关
        private bool _connectionState;
        /// <summary>
        /// PLC通讯状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool ConnectionState
        {
            get => _connectionState;
            set => SetProperty(ref _connectionState, value);
        }

        private int _connectionDelay;
        /// <summary>
        /// PLC通讯延时
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int ConnectionDelay
        {
            get => _connectionDelay;
            set => SetProperty(ref _connectionDelay, value);
        }

        private string _systempDataTime;
        /// <summary>
        /// 系统时间
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string SystempDataTime
        {
            get => _systempDataTime;
            set => SetProperty(ref _systempDataTime, value);
        }
        #endregion

        #region 控制信号
        private bool _heatRun;
        /// <summary>
        /// 自动加热启动信号
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool HeatRun
        {
            get => _heatRun;
            set => SetProperty(ref _heatRun, value);
        }

        private bool _coolingRun;
        /// <summary>
        /// 自动喷风启动信号
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool CoolingRun
        {
            get => _coolingRun;
            set => SetProperty(ref _coolingRun, value);
        }
        #endregion

        #region 电气参数
        private float _voltage;
        /// <summary>
        /// 电压
        /// </summary>
        public float Voltage
        {
            get => _voltage;
            set => SetProperty(ref _voltage, value);
        }

        private float _current;
        /// <summary>
        /// 电流
        /// </summary>
        public float Current
        {
            get => _current;
            set => SetProperty(ref _current, value);
        }

        private float _frequency;
        /// <summary>
        /// 频率
        /// </summary>
        public float Frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
        }

        private float _power;
        /// <summary>
        /// 功率
        /// </summary>
        public float Power
        {
            get => _power;
            set => SetProperty(ref _power, value);
        }

        private float _energy;
        /// <summary>
        /// 能量
        /// </summary>
        public float Energy
        {
            get => _energy;
            set => SetProperty(ref _energy, value);
        }
        #endregion

        #region 轴运动参数
        private float _xActPos;
        public float XActPos
        {
            get => _xActPos;
            set => SetProperty(ref _xActPos, value);
        }

        private float _xActSpeed;
        public float XActSpeed
        {
            get => _xActSpeed;
            set => SetProperty(ref _xActSpeed, value);
        }

        private float _xSetPos;
        public float XSetPos
        {
            get => _xSetPos;
            set => SetProperty(ref _xSetPos, value);
        }

        private float _xSetSpeed;
        public float XSetSpeed
        {
            get => _xSetSpeed;
            set => SetProperty(ref _xSetSpeed, value);
        }

        private float _xActCurrent;
        public float XActCurrent
        {
            get => _xActCurrent;
            set => SetProperty(ref _xActCurrent, value);
        }

        private float _yActPos;
        public float YActPos
        {
            get => _yActPos;
            set => SetProperty(ref _yActPos, value);
        }

        private float _yActSpeed;
        public float YActSpeed
        {
            get => _yActSpeed;
            set => SetProperty(ref _yActSpeed, value);
        }

        private float _ySetPos;
        public float YSetPos
        {
            get => _ySetPos;
            set => SetProperty(ref _ySetPos, value);
        }

        private float _ySetSpeed;
        public float YSetSpeed
        {
            get => _ySetSpeed;
            set => SetProperty(ref _ySetSpeed, value);
        }

        private float _yActCurrent;
        public float YActCurrent
        {
            get => _yActCurrent;
            set => SetProperty(ref _yActCurrent, value);
        }

        private float _zActPos;
        public float ZActPos
        {
            get => _zActPos;
            set => SetProperty(ref _zActPos, value);
        }

        private float _zActSpeed;
        public float ZActSpeed
        {
            get => _zActSpeed;
            set => SetProperty(ref _zActSpeed, value);
        }

        private float _zSetPos;
        public float ZSetPos
        {
            get => _zSetPos;
            set => SetProperty(ref _zSetPos, value);
        }

        private float _zSetSpeed;
        public float ZSetSpeed
        {
            get => _zSetSpeed;
            set => SetProperty(ref _zSetSpeed, value);
        }

        private float _zActCurrent;
        public float ZActCurrent
        {
            get => _zActCurrent;
            set => SetProperty(ref _zActCurrent, value);
        }
        #endregion

        #region 传感器数据
        private float _actTemp1;
        /// <summary>
        /// 温度1
        /// </summary>
        public float ActTemp1
        {
            get => _actTemp1;
            set => SetProperty(ref _actTemp1, value);
        }

        private float _actTemp2;
        /// <summary>
        /// 温度2
        /// </summary>
        public float ActTemp2
        {
            get => _actTemp2;
            set => SetProperty(ref _actTemp2, value);
        }

        private float _actTemp3;
        /// <summary>
        /// 温度3
        /// </summary>
        public float ActTemp3
        {
            get => _actTemp3;
            set => SetProperty(ref _actTemp3, value);
        }

        private float _actTemp4;
        /// <summary>
        /// 温度4
        /// </summary>
        public float ActTemp4
        {
            get => _actTemp4;
            set => SetProperty(ref _actTemp4, value);
        }

        private float _actFlow1;
        /// <summary>
        /// 流量1
        /// </summary>
        public float ActFlow1
        {
            get => _actFlow1;
            set => SetProperty(ref _actFlow1, value);
        }

        private float _actFlow2;
        /// <summary>
        /// 流量2
        /// </summary>
        public float ActFlow2
        {
            get => _actFlow2;
            set => SetProperty(ref _actFlow2, value);
        }

        private float _actFlow3;
        /// <summary>
        /// 流量3
        /// </summary>
        public float ActFlow3
        {
            get => _actFlow3;
            set => SetProperty(ref _actFlow3, value);
        }

        private float _actPressure;
        /// <summary>
        /// 压力
        /// </summary>
        public float ActPressure
        {
            get => _actPressure;
            set => SetProperty(ref _actPressure, value);
        }
        #endregion

        #region 报警信息
        private UInt16 _actAlarm1;
        /// <summary>
        /// 报警1
        /// </summary>
        public UInt16 ActAlarm1
        {
            get => _actAlarm1;
            set => SetProperty(ref _actAlarm1, value);
        }

        private UInt16 _actAlarm2;
        /// <summary>
        /// 报警2
        /// </summary>
        public UInt16 ActAlarm2
        {
            get => _actAlarm2;
            set => SetProperty(ref _actAlarm2, value);
        }
        #endregion

        #region 长度计量
        private float _actLenght1;
        /// <summary>
        /// 长度计1
        /// </summary>
        public float ActLenght1
        {
            get => _actLenght1;
            set => SetProperty(ref _actLenght1, value);
        }

        private float _actLenght2;
        /// <summary>
        /// 长度计2
        /// </summary>
        public float ActLenght2
        {
            get => _actLenght2;
            set => SetProperty(ref _actLenght2, value);
        }
        #endregion

        #region 气缸状态
        private bool _y_CDJ_QGQW;
        /// <summary>
        /// Y轴长度计气缸前位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool Y_CDJ_QGQW
        {
            get => _y_CDJ_QGQW;
            set => SetProperty(ref _y_CDJ_QGQW, value);
        }

        private bool _y_CDJ_QGHW;
        /// <summary>
        /// Y轴长度计气缸后位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool Y_CDJ_QGHW
        {
            get => _y_CDJ_QGHW;
            set => SetProperty(ref _y_CDJ_QGHW, value);
        }

        private bool _z_CDJ_QGQW;
        /// <summary>
        /// Z轴长度计气缸前位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool Z_CDJ_QGQW
        {
            get => _z_CDJ_QGQW;
            set => SetProperty(ref _z_CDJ_QGQW, value);
        }

        private bool _z_CDJ_QGHW;
        /// <summary>
        /// Z轴长度计气缸后位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool Z_CDJ_QGHW
        {
            get => _z_CDJ_QGHW;
            set => SetProperty(ref _z_CDJ_QGHW, value);
        }
        #endregion

        #region 举料缸状态
        private bool _jLG_1_SW;
        /// <summary>
        /// 举料缸1上位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_1_SW
        {
            get => _jLG_1_SW;
            set => SetProperty(ref _jLG_1_SW, value);
        }

        private bool _jLG_1_XW;
        /// <summary>
        /// 举料缸1下位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_1_XW
        {
            get => _jLG_1_XW;
            set => SetProperty(ref _jLG_1_XW, value);
        }

        private bool _jLG_2_SW;
        /// <summary>
        /// 举料缸2上位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_2_SW
        {
            get => _jLG_2_SW;
            set => SetProperty(ref _jLG_2_SW, value);
        }

        private bool _jLG_2_XW;
        /// <summary>
        /// 举料缸2下位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_2_XW
        {
            get => _jLG_2_XW;
            set => SetProperty(ref _jLG_2_XW, value);
        }

        private bool _jLG_3_SW;
        /// <summary>
        /// 举料缸3上位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_3_SW
        {
            get => _jLG_3_SW;
            set => SetProperty(ref _jLG_3_SW, value);
        }

        private bool _jLG_3_XW;
        /// <summary>
        /// 举料缸3下位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_3_XW
        {
            get => _jLG_3_XW;
            set => SetProperty(ref _jLG_3_XW, value);
        }

        private bool _jLG_4_SW;
        /// <summary>
        /// 举料缸4上位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_4_SW
        {
            get => _jLG_4_SW;
            set => SetProperty(ref _jLG_4_SW, value);
        }

        private bool _jLG_4_XW;
        /// <summary>
        /// 举料缸4下位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool JLG_4_XW
        {
            get => _jLG_4_XW;
            set => SetProperty(ref _jLG_4_XW, value);
        }
        #endregion

        #region 设备状态
        private bool _dY_ZT;
        /// <summary>
        /// 电源状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool DY_ZT
        {
            get => _dY_ZT;
            set => SetProperty(ref _dY_ZT, value);
        }

        private bool _pQ_ZT;
        /// <summary>
        /// 喷气状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool PQ_ZT
        {
            get => _pQ_ZT;
            set => SetProperty(ref _pQ_ZT, value);
        }

        private bool _processOk;
        /// <summary>
        /// 工件加工完成
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool ProcessOk
        {
            get => _processOk;
            set => SetProperty(ref _processOk, value);
        }
        #endregion


        #region Control 设备状态
        private bool _totalStart;
        /// <summary>
        /// 总启动
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool TotalStart
        {
            get => _totalStart;
            set => SetProperty(ref _totalStart, value);
        }

        private bool _totalStop;
        /// <summary>
        /// 总停止
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool TotalStop
        {
            get => _totalStop;
            set => SetProperty(ref _totalStop, value);
        }

        private bool _mechanicalReset;
        /// <summary>
        /// 机械复位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool MechanicalReset
        {
            get => _mechanicalReset;
            set => SetProperty(ref _mechanicalReset, value);
        }

        private bool _alarmReset;
        /// <summary>
        /// 报警复位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool AlarmReset
        {
            get => _alarmReset;
            set => SetProperty(ref _alarmReset, value);
        }

        private bool _idleRun;
        /// <summary>
        /// 空运行
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IdleRun
        {
            get => _idleRun;
            set => SetProperty(ref _idleRun, value);
        }
        #endregion

        /// <summary>
        /// 冷却室工位
        /// </summary>
        private bool _coolingRoomStation;

        public bool CoolingRoomStation
        {
            get => _coolingRoomStation;
            set => SetProperty(ref _coolingRoomStation, value);
        }

        /// <summary>
        /// 输送机工位
        /// </summary>
        private bool _conveyorStation;

        public bool ConveyorStation
        {
            get { return _conveyorStation; }
            set { _conveyorStation = value; }
        }


    }
}
