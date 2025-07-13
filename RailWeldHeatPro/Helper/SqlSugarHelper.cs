using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.Helper
{
    public static class SqlSugarHelper
    {
        public static SqlSugarScope Db { get; set; }

        public static void AddSqlSugarSetup(DbType dbType, string connectString)
        {
            Db = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = connectString, //连接符字串
                DbType = dbType, //数据库类型
                IsAutoCloseConnection = true //不设成true要手动close
            },
                db =>
                {
                    //(A)全局生效配置点，一般AOP和程序启动的配置扔这里面 ，所有上下文生效
                    //调试SQL事件，可以删掉
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        //获取原生SQL推荐 5.1.4.63  性能OK
                        Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                    };
                });
        }
    }
}
