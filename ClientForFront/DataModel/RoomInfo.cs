using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
