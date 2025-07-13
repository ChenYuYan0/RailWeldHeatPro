using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.Models
{
    /// <summary>
    /// RailDataRead表映射类
    /// </summary>
    public class RailDataReadEntity
    {
        /// <summary>
        /// PLC变量名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public int OffsetByte { get; set; }

        /// <summary>
        /// 变量类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// plc变量地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 该变量是否保存到数据库
        /// </summary>
        public bool Save { get; set; }  

    }
}
