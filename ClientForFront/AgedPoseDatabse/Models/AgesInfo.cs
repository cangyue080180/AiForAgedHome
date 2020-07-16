using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgedPoseDatabse.Models
{
    public class AgesInfo
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string ContacterName { get; set; }

        [Phone]
        public string ContacterPhone { get; set; }

        [StringLength(20)]
        public string NurseName { get; set; }
        [StringLength(100)]
        public string Address { get; set; }

        public long RoomInfoId { get; set; }

        public RoomInfo RoomInfo { get; set; }
    }
}
