using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailWeldHeatPro.Helper
{
    /// <summary>
    /// 存放文件路径
    /// </summary>
    public static class GlobalPath
    {
        /// <summary>
        /// 操作员名称数据配置文件路径
        /// </summary>
        public static readonly string OperatorNamesPath = Application.StartupPath+ "\\Config\\Txt\\OperatorNames.txt";

        /// <summary>
        /// 产线名文件路径
        /// </summary>
        public static readonly string LineNamesPath = Application.StartupPath+ "\\Config\\Txt\\LineNames.txt"; 

        /// <summary>
        /// 班组名文件路径
        /// </summary>
        public static readonly string GroupNamesPath = Application.StartupPath+ "\\Config\\Txt\\GroupNames.txt"; 

        /// <summary>
        /// 上次登录用户信息路径
        /// </summary>
        public static readonly string LoginUserPath = Application.StartupPath+ "\\Config\\Json\\LoginUser.json"; 

        /// <summary>
        /// excel数据路径
        /// </summary>
        public static readonly string ReadPath = Application.StartupPath+ "\\Config\\Data\\RailDataRead.xlsx"; 



    }
}
