using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection.Model
{
    /// <summary>
    /// Fanuc伺服误差实体(诊断号300)
    /// </summary>
    public class FanucServo300
    {
        /// <summary>
        /// IP
        /// </summary>
        [Column(IsPrimary = true)]
        public string IP {  get; set; }
        /// <summary>
        /// 采集编号
        /// </summary>
        [Column(IsPrimary = true)]
        public string CycleNum {  get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Column(IsPrimary = true)]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public double DataX {  get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public double DataY { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public double DataZ { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
