using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class ServerInfoVM : ViewModelValidationBase
    {
        public ServerInfoVM()
        {
            ValidationKey(new string[] { nameof(Name), nameof(FactoryInfo),nameof(Ip) });
            this.PropertyChanged += delegate { IsBeginValidation = true; };
        }
        public long Id { get; set; }

        private string _name;
        [Required(ErrorMessage ="不能为空")]
        [StringLength(20,ErrorMessage ="最大字符长度为20")]
        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        private string _factoryInfo;
        [StringLength(100, ErrorMessage = "最大字符长度为20")]
        public string FactoryInfo
        {
            get => _factoryInfo;
            set => Set(nameof(FactoryInfo), ref _factoryInfo, value);
        }

        private byte _maxCameraCount;
        public byte MaxCameraCount
        {
            get => _maxCameraCount;
            set => Set(nameof(MaxCameraCount), ref _maxCameraCount, value);
        }

        [StringLength(15, ErrorMessage = "地址格式不正确")]
        public string Ip { get; set; }
    }
}
