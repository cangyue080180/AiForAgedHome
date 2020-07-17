using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgedPoseDatabse.Models
{
    [Table("ServerInfo")]
    public class ServerInfo
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string FactoryInfo { get; set; }
        public byte MaxCameraCount { get; set; }

        public List<CameraInfo> CameraInfos { get; set; }
    }
}
