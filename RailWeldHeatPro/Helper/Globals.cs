using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailWeldHeatPro.Helper
{
    public static class Globals
    {
        /// <summary>
        /// 操作员名称数据配置文件路径
        /// </summary>
        public static readonly string OperatorNamesPath = Application.StartupPath+ "\\Config\\Txt\\OperatorNames.txt"; //姓名存储文件路径
        public static readonly string LineNamesPath = Application.StartupPath+ "\\Config\\Txt\\LineNames.txt"; //姓名存储文件路径
        public static readonly string GroupNamesPath = Application.StartupPath+ "\\Config\\Txt\\GroupNames.txt"; //姓名存储文件路径
        public static readonly string LoginUserPath = Application.StartupPath+ "\\Config\\Json\\LoginUser.json"; //姓名存储文件路径



        //public static IniFile InitReadData = new IniFile(Application.StartupPath + "\\Config\\Txt\\OperatorName.txt");
    }
}
