using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class ServerInfo
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string FactoryInfo { get; set; }
        public byte MaxCameraCount { get; set; }

        [StringLength(15)]
        public string Ip { get; set; }

        public List<CameraInfo> CameraInfos { get; set; }
    }
}
