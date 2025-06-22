using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.Models
{
    public class MainWindowItem 
    {
        public string Name { get; set; }

        public object Content { get; set; }

        public MainWindowItem(string name,object content) {
            Name = name;
            Content = content;
        }



    }
}
