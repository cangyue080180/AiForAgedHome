using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class AgesInfoVM : ViewModelValidationBase
    {
        public AgesInfoVM()
        {
            ValidationKey(new string[] {nameof(Name),nameof(ContacterName),nameof(ContacterPhone),nameof(NurseName),nameof(Address) });
            this.PropertyChanged += delegate { IsBeginValidation = true; };
        }

        public long Id { get; set; }

        private string _name;
        [Required(ErrorMessage ="不能为空")]
        [StringLength(20,ErrorMessage ="最大长度为20字符")]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _contacterName;
        [StringLength(20, ErrorMessage = "最大长度为20字符")]
        public string ContacterName
        {
            get => _contacterName;
            set => Set(ref _contacterName, value);
        }

        private string _contacterPhone;
        [StringLength(11, ErrorMessage = "号码长度不正确")]
        public string ContacterPhone
        {
            get => _contacterPhone;
            set => Set(ref _contacterPhone, value);
        }

        private string _nurseName;
        [StringLength(20, ErrorMessage = "最大长度为20字符")]
        public string NurseName
        {
            get => _nurseName;
            set => Set(ref _nurseName, value);
        }

        private string _address;
        [StringLength(100, ErrorMessage = "最大长度为100字符")]
        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        public long RoomInfoId { get; set; }

        private RoomInfoVM roomInfo;
        public RoomInfoVM RoomInfo
        {
            get => roomInfo;
            set => Set(ref roomInfo, value);
        }
    }
}
