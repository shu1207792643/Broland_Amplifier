using Autofac;
using Broland_Amplifier_Wpf.View;
using System;
using System.Windows.Controls;

namespace Broland_Amplifier_Wpf.Helper
{
    public class InitAutofac
    {
        //申明容器
        static ContainerBuilder _Builder;
        //申明一个字段这个字段用来对接容器
        static IContainer _container;
        //将对接的内容传输入这个属性！
        static IContainer Container 
        {
            get
            {
                if (_container == null)
                {
                    _container = _Builder.Build();
                }
                return _container;
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        public static void InitAutofacs()
        {
            //实例化
            _Builder = new ContainerBuilder();
            //注入页面信息
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("带内增益");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("带内增益平坦度");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("输出功率1dB压缩点");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("饱和输入功率");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("反向隔离度");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("输入/输出端口驻波");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("噪声系数");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("谐波抑制");
            _Builder.RegisterType<InBandGainView>().Named<UserControl>("衰减量");
        }
        /// <summary>
        /// 根据名称获取到注入的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">注入的名称</param>
        /// <returns></returns>
        public static T GetFromFac<T>(string name)
        {
            try
            {
                if (Container == null)
                {
                    InitAutofacs();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("IOC实例化出错!" + ex.Message);
            }

            return Container.ResolveNamed<T>(name);
        }
    }
}
