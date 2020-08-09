using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class ServerInfoVM : ViewModelBase
    {
        public long Id { get; set; }

        private string _name;
        [StringLength(20)]
        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        private string _factoryInfo;
        [StringLength(100)]
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

        [StringLength(15)]
        public string Ip { get; set; }
    }
}
