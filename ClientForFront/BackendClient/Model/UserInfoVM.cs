using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class UserInfoVM : ViewModelBase
    {
        public int Id { get; set; }

        private string _name;
        [StringLength(30)]
        public string Name
        {
            get => _name;
            set => Set(() => Name, ref _name, value);
        }

        private string _password;
        [StringLength(16)]
        public string Password
        {
            get => _password;
            set => Set(() => Password, ref _password, value);
        }

        private int _authority = 30;
        public int Authority
        {
            get => _authority;
            set => Set(() => Authority, ref _authority, value);
        }
    }
}
