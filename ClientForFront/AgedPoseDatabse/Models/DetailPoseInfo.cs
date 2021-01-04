using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgedPoseDatabse.Models
{
    /// <summary>
    /// 用来存储被监护人的详细状态记录信息，某人开始一个动作的时间会被记录下来。
    /// </summary>
    [Table("DetailPoseInfo")]
    public class DetailPoseInfo
    {
        public long AgesInfoId { get; set; }

        public AgesInfo AgesInfo { get; set; }

        public DateTime DateTime { get; set; }

        public byte? Status { get; set; }
    }
}