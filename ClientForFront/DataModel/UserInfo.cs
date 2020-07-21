using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class UserInfo
    {
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(16)]
        public string Password { get; set; }

        public int Authority { get; set; }
    }
}
