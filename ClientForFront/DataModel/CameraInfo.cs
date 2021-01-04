using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class CameraInfo
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string FactoryInfo { get; set; }

        [StringLength(15)]
        public string IpAddress { get; set; }

        [StringLength(100)]
        public string VideoAddress { get; set; }

        public long ServerInfoId { get; set; }

        public ServerInfo ServerInfo { get; set; }

        public long RoomInfoId { get; set; }
        public RoomInfo RoomInfo { get; set; }

        public bool IsUseSafeRegion { get; set; }
        public int LeftTopPointX { get; set; }
        public int LeftTopPointY { get; set; }
        public int RightBottomPointX { get; set; }
        public int RightBottomPointY { get; set; }
    }
}