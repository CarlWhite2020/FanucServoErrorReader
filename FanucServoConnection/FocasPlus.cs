using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public partial class Focas1
{
    /*诊断号数据读取,添加所有轴数据一次读取，分为按字节、整型、双浮点格式 */

    /// <summary>
    /// 按照字节读取诊断号数据
    /// </summary>
    /// <param name="FlibHndl">句柄</param>
    /// <param name="a">诊断号</param>
    /// <param name="b">轴编号,-1表示读取所有轴</param>
    /// <param name="c">读取数据区长度</param>
    /// <param name="d">返回数据体</param>
    /// <returns></returns>
    [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
    public static extern short cnc_diagnoss(ushort FlibHndl,
    short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_E d);
    /// <summary>
    /// 按照整型读取诊断号数据
    /// </summary>
    /// <param name="FlibHndl">句柄</param>
    /// <param name="a">诊断号</param>
    /// <param name="b">轴编号,-1表示读取所有轴</param>
    /// <param name="c">读取数据区长度</param>
    /// <param name="d">返回数据体</param>
    /// <returns></returns>
    [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
    public static extern short cnc_diagnoss(ushort FlibHndl,
        short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_C d);
    /// <summary>
    /// 按照双浮点读取诊断号数据
    /// </summary>
    /// <param name="FlibHndl">句柄</param>
    /// <param name="a">诊断号</param>
    /// <param name="b">轴编号,-1表示读取所有轴</param>
    /// <param name="c">读取数据区长度</param>
    /// <param name="d">返回数据体</param>
    /// <returns></returns>
    [DllImport("FWLIB32.dll", EntryPoint = "cnc_diagnoss")]
    public static extern short cnc_diagnoss(ushort FlibHndl,
        short a, short b, short c, [Out, MarshalAs(UnmanagedType.LPStruct)] ODBDGN_D d);
    /// <summary>
    /// 字节格式映射返回数据体
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class ODBDGN_5
    {
        [FieldOffset(0)]
        public short datano;    /* data number */
        [FieldOffset(2)]
        public short type;      /* axis number */
        [FieldOffset(4),
       MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AXIS)]
        public byte[] cdatas;
    }
    /// <summary>
    /// 字节数据体结合
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class ODBDGN_E
    {
        public ODBDGN_5 data1 = new ODBDGN_5();
        public ODBDGN_5 data2 = new ODBDGN_5();
        public ODBDGN_5 data3 = new ODBDGN_5();
        public ODBDGN_5 data4 = new ODBDGN_5();
        public ODBDGN_5 data5 = new ODBDGN_5();
        public ODBDGN_5 data6 = new ODBDGN_5();
        public ODBDGN_5 data7 = new ODBDGN_5();
    } /* (sample) must be modified */
}
