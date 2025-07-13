using CommunityToolkit.Mvvm.ComponentModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.Models
{
    public class EntityBase : ObservableObject
    {
		private int _id;

        // 使用 SugarColumn 属性来标记 Id 属性，将其指定为主键，并且设置为自增列
        // IsPrimaryKey = true 表示该属性是数据库表的主键
        // IsIdentity = true 表示该属性的值会自动递增
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
		{
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private DateTime _createTime = DateTime.Now;
        public DateTime CreateTime
        {
            get => _createTime;
            set => SetProperty(ref _createTime, value);
        }

        private DateTime _updateTime = DateTime.Now;

        public DateTime UpdateTime
        {
            get => _updateTime;
            set => SetProperty(ref _updateTime, value);
        }

    }
}
