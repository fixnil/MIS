using GISCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISWinApp
{
    public partial class MainFrm : Form
    {
        // 是否报警
        private bool _isWarning = false;

        // 报警任务
        private Task _warningTask;
        // Web 服务器任务
        private Task _webServerTask;
        // Web 服务器进程
        private Process _webServerProcess;

        // 默认背景颜色
        private Color _defaultBackColor;
        // 当前串口
        private SerialPort _currentSerial;

        public MainFrm()
        {
            InitializeComponent();

            _defaultBackColor = this.BackColor;

            CheckForIllegalCrossThreadCalls = false;
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            LogFrm log = new LogFrm(dtStart.Value, dtEnd.Value);

            log.ShowDialog();
        }

        private void MainFrm_Shown(object sender, EventArgs e)
        {
            this.SysSetting();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                _currentSerial = new SerialPort
                {
                    // 波特率
                    BaudRate = Int32.Parse(txtBaudRate.Text),
                    // 串口名
                    PortName = txtCom.Text,
                    // 数据位
                    DataBits = 8
                };
                // 打开串口
                _currentSerial.Open();

                this.ShowMsg("\r\n打开串口成功!");

                // 订阅读取事件
                _currentSerial.DataReceived += new SerialDataReceivedEventHandler(ReadSerial);
                this.ShowMsg("监听串口成功...");

                btnOpen.Enabled = false;
            }
            catch (Exception)
            {
                //
                this.ShowMsg("打开串口失败!");
            }
        }

        private void ToolLoadWeb_Click(object sender, EventArgs e)
        {
            this.CloseWebServer();

            LogHelper.ReSelectWeb();

            this.SysSetting();
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CloseWebServer();
        }

        /// <summary>
        /// 循环接受串口的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadSerial(object sender, SerialDataReceivedEventArgs e)
        {
            if (_currentSerial != null && _currentSerial.IsOpen)
            {
                try
                {
                    // 接收数据
                    var data = _currentSerial.ReadTo("$");

                    _currentSerial.DiscardInBuffer();

                    this.AnalyzeData(data);
                }
                catch (TimeoutException)
                {
                    //
                    this.ShowMsg("接收串口数据失败!");
                }
            }
            else
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="data"></param>
        public void AnalyzeData(string data)
        {
            this.ShowMsg("收到串口数据：" + data);

            var dataArray = data.Split(',');

            if (dataArray.Length == 4 || dataArray.Length == 5)
            {
                this.ShowMsg("解析数据成功!");

                var info = new Info
                {
                    Temp = dataArray[0],
                    Hum =  dataArray[1],
                    NH3 = dataArray[2],
                    Light =  dataArray[3],
                    Time = DateTime.Now
                };

                this.SaveData(info);

                this.PrintStatus(info);

                this.HandleWarning(dataArray.Length == 5 ? dataArray[4] : null);
            }
            else
            {
                this.ShowMsg("解析数据失败!");
            }
        }



        /// <summary>
        /// 显示当前状态
        /// </summary>
        /// <param name="info"></param>
        public void PrintStatus(Info info)
        {
            labTemp.Text = info.Temp + GISConst.Temp;
            labHum.Text = info.Hum + GISConst.Hum;
            labNH3.Text = info.NH3 + GISConst.NH3;
            labLight.Text = info.Light + GISConst.Light;
        }

        /// <summary>
        /// 火灾报警处理
        /// </summary>
        /// <param name="isWarning">数据</param>
        public void HandleWarning(string isWarning)
        {
            if (string.IsNullOrWhiteSpace(isWarning)) return;

            _isWarning = isWarning == "1" ? true : false;

            if (_warningTask != null && _warningTask.Status == TaskStatus.Running)
            {
                if (!_isWarning)
                {
                    _warningTask.Wait();

                    this.BackColor = _defaultBackColor;

                    this.ShowMsg("警报解除...");
                }
            }

            if (!_isWarning) return;

            if (_warningTask == null || _warningTask.Status != TaskStatus.Running)
            {
                _warningTask = Task.Run(() =>
                {
                    // 背景闪烁
                    while (_isWarning)
                    {
                        this.BackColor = Color.Red;

                        this.ShowMsg("火灾警报!!!");

                        Thread.Sleep(300);

                        this.BackColor = _defaultBackColor;

                        Thread.Sleep(300);
                    }
                });
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="info">元数据</param>
        public void SaveData(Info info)
        {
            using (GISContext context = new GISContext())
            {
                try
                {
                    context.Infoes.Add(info);

                    context.SaveChanges();

                    this.ShowMsg("保存数据成功!");
                }
                catch (Exception ex)
                {
                    this.ShowMsg("保存数据失败! " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMsg(string message)
        {
            txtMsg.AppendText(Environment.NewLine + message);
        }

        /// <summary>
        /// 关闭 Web 服务器
        /// </summary>
        private void CloseWebServer()
        {
            if (_webServerProcess != null && !_webServerProcess.HasExited)
            {
                _webServerProcess.Kill();
            }

            if (_webServerTask != null)
            {
                _webServerTask.Wait();
            }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        private void SysSetting()
        {
            if (!this.SysConfig()) return;

            this.StartWebServer();

            this.OpenBrowser();
        }

        /// <summary>
        /// 配置 Web 和数据库
        /// </summary>
        /// <param name="isrb">是否是递归调用</param>
        /// <returns></returns>
        public bool SysConfig(bool isrb = false)
        {
            var isSuccess = LogHelper.IsSuccess;

            if (isrb && !isSuccess)
            {
                this.ShowSysError();

                return false;
            }

            if (!isSuccess)
            {
                if (!LogHelper.IsFirstTime) this.ShowSysError();

                new InitFrm().ShowDialog();

                return this.SysConfig(true);
            }

            return true;
        }

        /// <summary>
        /// 显示系统配置错误
        /// </summary>
        public void ShowSysError()
        {
            MessageBox.Show("好像遇到了麻烦，系统配置应该不对。。。。", "配置错误");
        }

        /// <summary>
        /// 启动 Web 服务器
        /// </summary>
        private void StartWebServer()
        {
            // 先尝试关闭 Web 服务器
            this.CloseWebServer();

            var path = Path.Combine(AppContext.BaseDirectory, GISConst.WebRoot);

            _webServerTask = Task.Run(() =>
            {
                // 实例化 Web 服务器进程并设置进程信息
                _webServerProcess = new Process
                {
                    StartInfo = new ProcessStartInfo(Path.Combine(path, GISConst.WebAppName), path)
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    }
                };

                // 启动 Web 服务器
                _webServerProcess.Start();

                while (!_webServerProcess.HasExited)
                {
                    // 此处会阻塞在 ReadLine 中.
                    // 所以每当 _process 的输出流可读时便会读取并打印到信息窗口
                    txtMsg.AppendText(Environment.NewLine + _webServerProcess.StandardOutput.ReadLine());
                }
            });
        }

        /// <summary>
        /// 打开浏览器
        /// </summary>
        private void OpenBrowser()
        {
            Task.Run(() =>
            {
                var p = Process.Start("cmd", GISConst.command);

                p.WaitForExit(1000);
            });
        }
    }
}