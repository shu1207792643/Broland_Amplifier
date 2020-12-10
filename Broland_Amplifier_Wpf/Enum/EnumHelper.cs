using System.ComponentModel;

namespace Broland_Amplifier_Wpf.Enum
{
    public static class EnumHelper
    {
        /// <summary>
        /// 弹窗枚举
        /// </summary>
        public enum OpenType
        {
            [Description("正常弹窗")]
            Show = 1,
            [Description("子窗口弹窗")]
            ShowDialog = 2
        }
    }
}
