using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Broland_Amplifier_Wpf.Helper
{
    public class SocketHttpHelper
    {
        private string ip = "127.0.0.1";
        private int port = 8123;
        private int count = 0;
        private Socket server = null;

        public string DefaultReturn = string.Empty;

        public event Func<string, string, string> Handler = null;

        public SocketHttpHelper()
        {
        }

        public SocketHttpHelper(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="count"></param>
        public void StartListen(int count = 10)
        {
            this.count = count;
            Thread t = new Thread(new ThreadStart(ProcessThread));
            t.IsBackground = true;
            t.Start();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                server.Close();
            }
            catch { }
        }

        private void ProcessThread()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port));
            server.Listen(count);
            while (true)
            {
                try
                {
                    Socket client = server.Accept();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ListenExecute), client);
                }
                catch { }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 监听和执行
        /// </summary>
        /// <param name="obj"></param>
        private void ListenExecute(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                string ip = (client.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString();
                byte[] buffer = new byte[1024];
                int count = client.Receive(buffer);
                if (count > 0)
                {
                    string content = Encoding.UTF8.GetString(buffer, 0, count);

                    // 解析 content
                    Regex actionRegex = new Regex(@"GET\s+/(?<action>\w+)\?(?<args>[^\s]{0,})");
                    string action = actionRegex.Match(content).Groups["action"].Value;
                    string args = actionRegex.Match(content).Groups["args"].Value;
                    string headStr = @"HTTP/1.0 200 OK Content-Type: text/html Connection: keep-alive Content-Encoding: utf-8";
                    if (Handler != null)
                    {
                        try
                        {
                            string result = Handler(action, args);
                            string data = string.Format(headStr + result);
                            client.Send(Encoding.UTF8.GetBytes(data));
                        }
                        catch { }
                        finally
                        {
                        }
                    }
                    else
                    {
                        string data = string.Format(headStr + DefaultReturn);
                        client.Send(Encoding.UTF8.GetBytes(data));
                    }
                }
            }
            catch { }
            finally
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    client.Dispose();
                }
                catch { }
            }
        }
    }
}
