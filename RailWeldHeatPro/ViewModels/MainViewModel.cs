using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RailWeldHeatPro.Models;
using RailWeldHeatPro.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private MainWindowItem[] _demoItems;

        public MainViewModel() {

            InitMainViewDemoItems();
        }
        /// <summary>
        /// 初始化主菜单侧边栏选项
        /// </summary>
        private void InitMainViewDemoItems()
        {
            MachineToolView machineToolView = App.Current.ServiceProviders.GetService<MachineToolView>();

            DemoItems = new MainWindowItem[] {
                new MainWindowItem("机床信息", machineToolView )

            };
        }
    }
}
