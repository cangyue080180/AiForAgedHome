using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class UserInfo
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(16)]
        public string Password { get; set; }

        public int Authority { get; set; }
    }
}
