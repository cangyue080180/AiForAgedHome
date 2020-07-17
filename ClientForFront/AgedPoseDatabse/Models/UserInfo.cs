using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgedPoseDatabse.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(16)]
        public string Password { get; set; }

        public int Authority { get; set; }
    }
}
