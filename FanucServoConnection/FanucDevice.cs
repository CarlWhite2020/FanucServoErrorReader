using FanucServoConnection.Log;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FanucServoConnection
{
    /// <summary>
    /// Fanuc设备操作类
    /// </summary>
    public class FanucDevice
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public short Ret {  get; set; }
        /// <summary>
        /// 句柄
        /// </summary>
        public ushort Handle;
        /// <summary>
        /// 标识是否已连接
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// 循环是否开始信号
        /// </summary>
        public bool CycleEnable { get; set; } = false;
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public short Connect(string ip)
        {
            Ret= Focas1.cnc_allclibhndl3(ip,8193,5, out Handle);
            return Ret;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            Focas1.cnc_freelibhndl(Handle);
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Debug.WriteLine($"{time} 断开设备连接");
        }
        /// <summary>
        /// 读取单个地址字节类型数据
        /// </summary>
        /// <param name="type">地址类型</param>
        /// <param name="num">地址号</param>
        /// <returns></returns>
        public byte ReadPmcByteData(FanucPMCAddrType type,ushort num)
        {
            Focas1.IODBPMC0 iODBPMC0 = new Focas1.IODBPMC0();
            Ret = Focas1.pmc_rdpmcrng(Handle, (short)type, 0, num, (ushort)(num + 1), (ushort)(8 + 2), iODBPMC0);
            if (Ret == 0) {
                var data = iODBPMC0.cdata[0];
                return data;
            }
            else
            {
                Logger.Error($"读取PMC地址错误:{Ret}");
                return 0;
            }
        }

        /// <summary>
        /// 读取多个轴诊断号数据(适用于单个轴数据为整型格式情况)
        /// 最多可以返回9个轴数据
        /// </summary>
        /// <param name="num">诊断号</param>
        /// <param name="axis">轴编号,-1表示读取所有轴</param>
        /// <remarks>按照字节解析回传数据,一个轴数据占用1个int,共计4字节</remarks>
        /// <returns>ret-状态码,data-轴数据</returns>
        public int[] ReadDiagnossIntData(short num, short axis = -1)
        {
            Focas1.ODBDGN_D diagnoss = new Focas1.ODBDGN_D();
            Ret = Focas1.cnc_diagnoss(Handle, num, axis, short.MaxValue, diagnoss);
            if (Ret == 0)
            {
                int[] data;
                if (axis == -1)
                {
                    data = new int[]
                    {
                        diagnoss.data1.rdatas.rdata1.dgn_val,
                        diagnoss.data1.rdatas.rdata1.dec_val,
                        diagnoss.data1.rdatas.rdata2.dgn_val,
                        diagnoss.data1.rdatas.rdata2.dec_val,
                        diagnoss.data1.rdatas.rdata3.dgn_val,
                        diagnoss.data1.rdatas.rdata3.dec_val,
                        diagnoss.data1.rdatas.rdata4.dgn_val,
                        diagnoss.data1.rdatas.rdata4.dec_val,
                    };
                    return data;
                }
                else
                {
                    data = new int[] { diagnoss.data1.rdatas.rdata1.dgn_val };
                    return data;
                }
            }
            else
            {
                Logger.Error($"读取诊断号地址[{num}]错误:{Ret}");
                return new int[] { 0 };
            }
        }

    }


    /// <summary>
    /// PMC地址类型
    /// </summary>
    public enum FanucPMCAddrType
    {
        /// <summary>
        /// Signal to PMC->CNC
        /// </summary>
        G = 0,
        /// <summary>
        /// Signal to CNC->PMC
        /// </summary>
        F = 1,
        /// <summary>
        /// Signal to PMC->machine
        /// </summary>
        Y = 2,
        /// <summary>
        /// Signal to machine->PMC
        /// </summary>
        X = 3,
        /// <summary>
        /// Message demand
        /// </summary>
        A = 4,
        /// <summary>
        /// Internal relay
        /// </summary>
        R = 5,
        /// <summary>
        /// Changeable time
        /// </summary>
        T = 6,
        /// <summary>
        /// Keep relay
        /// </summary>
        K = 7,
        /// <summary>
        /// Counter
        /// </summary>
        C = 8,
        /// <summary>
        /// Data table
        /// </summary>
        D = 9
    }

    /// <summary>
    /// PMC数据类型
    /// </summary>
    public enum FanucPMCDataType
    {
        /// <summary>
        /// 字节数据
        /// </summary>
        Byte = 0,
        /// <summary>
        /// 字数据,双字节
        /// </summary>
        Word = 1,
        /// <summary>
        /// 长整型,四字节
        /// </summary>
        Long = 2
    }
}
