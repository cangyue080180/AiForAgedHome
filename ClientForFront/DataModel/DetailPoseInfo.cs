using System;

namespace DataModel
{
    public class DetailPoseInfo
    {
        public long AgesInfoId { get; set; }

        public AgesInfo AgesInfo { get; set; }

        public DateTime DateTime { get; set; }

        public byte? Status { get; set; }
    }
}