using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RailWeldHeatPro.Ucs
{
    /// <summary>
    /// StatusControl.xaml 的交互逻辑
    /// </summary>
    public partial class StatusControl : UserControl
    {
        public StatusControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 按钮的背景颜色，默认绿色
        /// </summary>
        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonBackgroundProperty =
            DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(StatusControl), new PropertyMetadata(Brushes.Green));


        /// <summary>
        /// 转圈动画是否显示，默认停止
        /// </summary>
        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsIndeterminate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(StatusControl), new PropertyMetadata(false));


        /// <summary>
        /// 状态文本属性，默认为空
        /// </summary>

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(UserControl), new PropertyMetadata(string.Empty));


    }
}
