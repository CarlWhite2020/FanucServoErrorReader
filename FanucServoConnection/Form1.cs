using FanucServoConnection.Log;
using FanucServoConnection.Model;
using FanucServoConnection.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanucServoConnection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            TraceLog("程序启动", true);
            FanucThread = new Thread(new ThreadStart(FanucDeviceTask));
            FanucThread.Start();
        }
        /// <summary>
        /// 是否在采集中
        /// </summary>
        public static bool IsRunning { get; set; } = false;
        /// <summary>
        /// 是否退出程序
        /// </summary>
        public static bool IsExit { get; set; } = false;

        private void button1_Click(object sender, EventArgs e)
        {
            IsRunning = !IsRunning;
            if (IsRunning)
            {
                SwitchToRunning();
            }
            else
            {
                SwitchToStop();
            }
        }
        /// <summary>
        /// Fanuc扫描线程
        /// </summary>
        public Thread FanucThread;
        /// <summary>
        /// Fanuc设备操作类
        /// </summary>
        FanucDevice device = new FanucDevice();
        private void FanucDeviceTask()
        {
            while (IsExit == false)
            {
                DateTime scanStart = DateTime.Now;
                //Debug.WriteLine($"守护线程：{cycleStart:yyyy-MM-dd HH:mm:ss.fff}");
                if (IsRunning)
                {
                    string ip = Tbx_IP.Text.Trim();
                    device.Connect(ip);
                    if (device.Ret == 0)
                    {
                        while (true)
                        {
                            if (IsRunning == false)
                            {
                                device.DisConnect();
                                break;
                            }

                            var flag = ReadCycleStartFlag();
                            if (device.Ret != 0)
                            {
                                TraceLog("设备PMC数据读取失败,取消采集", true);
                                device.DisConnect();
                                break;
                            }

                            WriteLine("读取循环开始信号:" + flag.ToString());
                            //第一次循环可能不完整，不做采集
                            if (!flag)
                            {
                                if (device.CycleEnable == false)
                                {
                                    TraceLog("采集初始化成功，等待循环开始");
                                }
                                device.CycleEnable = true;
                                Thread.Sleep(100);
                            }

                            if (flag && device.CycleEnable)
                            {
                                DateTime cycleStart = DateTime.Now;
                                string cyclenum = cycleStart.ToString("yyyyMMddHHmmss");
                                TraceLog("循环启动,开始采集,采集批次:" + cyclenum);

                                //采集伺服误差数据
                                bool readComp = true;//是否采集伺服数据完成
                                List<FanucServo300> sercoList = new List<FanucServo300>();
                                for (int i = 0; i < 30000; i++)
                                {
                                    if (IsRunning == false)
                                    {
                                        device.DisConnect();
                                        readComp = false;
                                        break;
                                    }

                                    //采集一次数据
                                    var diag = ReadServoError();
                                    if (device.Ret != 0)
                                    {
                                        readComp = false;
                                        TraceLog("设备诊断号数据读取失败,取消采集批次:" + cyclenum, true);
                                        break;
                                    }
                                    else
                                    {
                                        FanucServo300 servodata = new FanucServo300();
                                        servodata.IP = ip;
                                        servodata.CycleNum = cyclenum;
                                        servodata.DataX = diag[0];
                                        servodata.DataY = diag[1];
                                        servodata.DataZ = diag[2];
                                        servodata.TimeStamp = DateTime.Now;
                                        sercoList.Add(servodata);
                                        WriteLine($"第{i}次读取诊断号300数据:" + string.Join(",", diag));
                                    }

                                    //判断是否继续
                                    var running = ReadCycleStartFlag();
                                    if (device.Ret == 0)
                                    {
                                        //WriteLine("读取循环运行信号:" + running.ToString());
                                        if (!running)
                                        {
                                            break;//循环结束，采集完成
                                        }
                                    }
                                    else
                                    {
                                        TraceLog("设备运行状态读取失败,取消采集批次:" + cyclenum, true);
                                        break;
                                    }
                                }

                                //保存数据
                                if (readComp)
                                {
                                    FanucCycle cycle = new FanucCycle();
                                    cycle.IP = ip;
                                    cycle.CycleNum = cyclenum;
                                    cycle.CycleType = "/cnc_diagnoss/300";
                                    cycle.CreateDate = DateTime.Now;
                                    FanucCycleService cycleService = new FanucCycleService();
                                    cycleService.InsertAsync(cycle);

                                    FanucServoService servoService = new FanucServoService();
                                    servoService.InsertAsync(sercoList);
                                    TraceLog("伺服误差采集完成,数据批次:" + cycle.CycleNum, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        TraceLog("设备无法连接:" + ip + ",等待重新连接");
                        Thread.Sleep(3000);
                    }
                }

                //循环休息时间控制
                var duration = (int)((DateTime.Now - scanStart).TotalMilliseconds);
                if (duration < 300)
                {
                    Thread.Sleep(300);
                }
            }
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        List<string> TraceList = new List<string>();
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="toFile">记录到文件</param>
        /// <param name="type"></param>
        public void TraceLog(string txt, bool toFile = false, string type = "INFO")
        {
            Task.Run(() =>
            {
                if (TraceList.Count > 200)
                {
                    TraceList.RemoveAt(0);
                }
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string content = time + " [" + type + "] " + txt + "\r\n";

                Tbx_IP.Invoke(new Action(() =>
                {
                    TraceList.Add(content);
                    Tbx_Trace.Text = "";
                    foreach (string line in TraceList)
                    {
                        Tbx_Trace.AppendText(line);
                    }
                }));

                if (toFile)
                {
                    Logger.Info(content);
                }
            });

        }

        /// <summary>
        /// 读取循环开始信号R8.7的值
        /// </summary>
        /// <returns></returns>
        public bool ReadCycleStartFlag()
        {
            var data = device.ReadPmcByteData(FanucPMCAddrType.R, 8);
            if (device.Ret == 0)
            {
                // 通过掩码提取第 7 位
                bool bit = (data & (1 << 7)) != 0;
                return bit;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 读取伺服误差数据(诊断号300)
        /// </summary>
        /// <returns></returns>
        public int[] ReadServoError()
        {
            var data = device.ReadDiagnossIntData(300);
            if (device.Ret == 0)
            {
                int[] servo = new int[]
                {
                    data[0],
                    data[1],
                    data[2],
                };
                return servo;
            }
            else
            {
                return new int[0];
            }
        }
        /// <summary>
        /// 切换到采集状态
        /// </summary>
        public void SwitchToRunning()
        {
            Tbx_IP.Invoke(new Action(() =>
            {
                Btn_Start.Text = "停止";
                Tbx_IP.Enabled = false;
                TraceLog("开始数据采集作业");
            }));
        }
        /// <summary>
        /// 切换到停止采集状态
        /// </summary>
        public void SwitchToStop()
        {
            Tbx_IP.Invoke(new Action(() =>
            {
                Btn_Start.Text = "开始采集";
                Tbx_IP.Enabled = true;
                device.CycleEnable = false;
                TraceLog("停止数据采集作业");
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsRunning = false;
            IsExit = true;
            SwitchToStop();
        }

        public void WriteLine(string txt)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Debug.WriteLine($"{time} {txt}");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            FanucCycle cycle = new FanucCycle();
            cycle.IP = Tbx_IP.Text;
            cycle.CycleNum = time;
            cycle.CycleType = "/cnc_diagnoss/300";
            cycle.CreateDate = DateTime.Now;
            FanucCycleService cycleService = new FanucCycleService();
            cycleService.InsertAsync(cycle);

            List<FanucServo300> list = new List<FanucServo300>();
            for (int i = 0; i < 1000; i++)
            {
                FanucServo300 servo = new FanucServo300();
                servo.IP = Tbx_IP.Text;
                servo.CycleNum = time;

            }
        }
    }
}
