using System;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class PoseInfo
    {
        public long AgesInfoId { get; set; }
        public AgesInfo AgesInfo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int TimeStand { get; set; }

        public int TimeSit { get; set; }

        public int TimeLie { get; set; }

        public int TimeDown { get; set; }

        public int TimeOther { get; set; }

        [StringLength(8)]
        public string TimeIn { get; set; }

        public bool IsAlarm { get; set; }

        public byte? Status { get; set; }
    }
}
