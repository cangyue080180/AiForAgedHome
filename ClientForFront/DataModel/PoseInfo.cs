using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TimeSpan TimeIn { get; set; }

        public bool IsAlarm { get; set; }
    }
}
