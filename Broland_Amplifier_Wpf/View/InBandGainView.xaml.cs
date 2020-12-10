using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO.Ports;
using Broland_Amplifier_Wpf.Helper;
using Broland_Amplifier_Wpf.Model;
using System.Text;
using System.Threading;

namespace Broland_Amplifier_Wpf.View
{
    /// <summary>
    /// InBandGainView.xaml 的交互逻辑
    /// </summary>
    public partial class InBandGainView : UserControl
    {

        //初始化串口
        public SerialPort _serialPort = new SerialPort();

        //可变字符串类，用于存储接收到的字符
        private StringBuilder _builder = new StringBuilder();

        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();

        private DispatcherTimer timer = new DispatcherTimer();

        Random random = new Random();

        private int i = 0;

        bool stop = false;//用于曲线暂停测试，stop为true时停止画线
        public InBandGainView()
        {
            InitializeComponent();

            ssssss();
        }

        private void AnimatedPlot(object sender, EventArgs e)
        {
            double x = i;
            double y = random.Next(1, 1000);

            Point point = new Point(x, y);
            dataSource.AppendAsync(base.Dispatcher, point);

            cpuUsageText.Text = String.Format("{0:0}%", y);
            i++;
        }

        private void ssssss()
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.DefaultExt = ".xlsx";
            //sfd.AddExtension = true;
            //sfd.Filter = "Excel 2010 文件(.xlsx)|*.xlsx";
            //sfd.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            //if (sfd.ShowDialog() == true)
            //{
            //    var ser = sfd.FileName;
            //}
            plotter.AddLineGraph(dataSource, Colors.Green, 2, "Percentage");
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(AnimatedPlot);
            timer.IsEnabled = stop;
            plotter.Viewport.FitToView();

            but1.Content = "开始";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /****
            * 测试曲线暂停
            * */
            stop = !stop;
            if (stop)
            {
                timer.IsEnabled = false;
                but1.Content = "开始";
            }
            else
            {
                timer.IsEnabled = true;
                but1.Content = "暂停";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenSerialPort();
        }

        private void OpenSerialPort()
        {

            _serialPort.PortName = "COM1";
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.NewLine = "/r/n";
            // 与设置RTS信号有关，虽不明，但觉厉，照着做
            _serialPort.RtsEnable = true;
            //注册对串口接收数据的响应方法
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(Comm_DataReceived);

            _serialPort.Open();
        }

        /// <summary>
        /// 接受端口返回参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //获取接收缓冲区中数据的字节数
            int n = _serialPort.BytesToRead;
            byte[] buf = new byte[n];

            //将数据读入buf数组中
            _serialPort.Read(buf, 0, n);

            ////先清空
            //_builder.Clear();

            //委托方法在txGet控件中显示接收到的字符
            _builder.Append(Encoding.ASCII.GetString(buf));

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _serialPort.Write("123456");
        }
    }
}
