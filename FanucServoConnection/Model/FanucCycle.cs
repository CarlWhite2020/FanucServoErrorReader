using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanucServoConnection.Model
{
    /// <summary>
    /// Fanuc循环采集清单
    /// </summary>
    public class FanucCycle
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true,IsPrimary =true)]
        public int Id { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP {  get; set; }
        /// <summary>
        /// 循环编号
        /// </summary>
        public string CycleNum {  get; set; }
        /// <summary>
        /// 循环类型
        /// </summary>
        public string CycleType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
