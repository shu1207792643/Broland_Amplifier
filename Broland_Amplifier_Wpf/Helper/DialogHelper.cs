using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static Broland_Amplifier_Wpf.Enum.EnumHelper;

namespace Broland_Amplifier_Wpf.Helper
{
    public class DialogHelper
    {
        /// <summary>
        /// 页面字典
        /// </summary>
        public static Dictionary<string, Window> Dic { get; set; }
        /// <summary>
        /// 页面打开
        /// </summary>
        /// <param name="NodeName">打开页面名称</param>
        /// <param name="OpenType">打开类型</param>
        public static void DialogOpen(string NodeName, OpenType OpenType)
        {
            Dic = new Dictionary<string, Window>();

            var SelectedView = InitAutofac.GetFromFac<Window>(NodeName);

            Dic.Add(NodeName, SelectedView);
            if (OpenType == OpenType.Show)
            {
                SelectedView.Show();
            }
            else
            {
                SelectedView.ShowDialog();
            }
        }
        /// <summary>
        /// 页面关闭
        /// </summary>
        /// <param name="NodeName">关闭页面名称</param>
        public static void DialogClose(string NodeName)
        {
            var SelectedView = Dic.First(s => s.Key == NodeName).Value;

            SelectedView.Close();
        }
    }
}
