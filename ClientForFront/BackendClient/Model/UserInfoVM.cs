using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class UserInfoVM : ViewModelValidationBase
    {
        public UserInfoVM()
        {
            ValidationKey(new string[] { nameof(Name), nameof(Password), nameof(Authority) });
            this.PropertyChanged += delegate { IsBeginValidation = true; };
        }

        public int Id { get; set; }

        private string _name;

        [Required(ErrorMessage = "不能为空")]
        [StringLength(30, ErrorMessage = "最大字符长度为30")]
        public string Name
        {
            get => _name;
            set => Set(() => Name, ref _name, value);
        }

        private string _password;

        [Required(ErrorMessage = "不能为空")]
        [StringLength(16, ErrorMessage = "最大字符长度为16")]
        public string Password
        {
            get => _password;
            set => Set(() => Password, ref _password, value);
        }

        private int _authority = 30;

        [Range(0, 34, ErrorMessage = "不正确值")]
        public int Authority
        {
            get => _authority;
            set => Set(() => Authority, ref _authority, value);
        }
    }
}