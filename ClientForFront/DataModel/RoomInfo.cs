using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class RoomInfo
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [Range(0, 500)]
        public int RoomSize { get; set; }

        public List<AgesInfo> AgesInfos { get; set; }

        public List<CameraInfo> CameraInfos { get; set; }
    }
}
