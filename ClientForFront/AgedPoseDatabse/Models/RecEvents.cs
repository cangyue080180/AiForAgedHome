using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgedPoseDatabse.Models
{
    /// <summary>
    /// 识别事件model
    /// </summary>
    [Table("RecEvent")]
    public class RecEvent
    {
        public long Id { get; set; }

        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 事件名称或描述
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public string Img { get; set; }

    }
}
