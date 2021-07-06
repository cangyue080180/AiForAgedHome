using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class AgesInfo
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [StringLength(20)]
        public string ContacterName { get; set; }

        [StringLength(11)]
        public string ContacterPhone { get; set; }

        [StringLength(20)]
        public string NurseName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public long RoomInfoId { get; set; }

        public RoomInfo RoomInfo { get; set; }

        public List<PoseInfo> PoseInfos { get; set; }
    }
}