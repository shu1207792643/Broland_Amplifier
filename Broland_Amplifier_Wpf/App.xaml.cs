using Broland_Amplifier_Wpf.Helper;
using System.Windows;

namespace Broland_Amplifier_Wpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitAutofac.InitAutofacs();
            base.OnStartup(e);
        }
    }
}
