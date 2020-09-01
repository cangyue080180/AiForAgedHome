using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;

namespace BackendClient.Model
{
    public class RoomInfoVM : ViewModelValidationBase
    {
        public RoomInfoVM()
        {
            ValidationKey(new string[] { nameof(Name), nameof(RoomSize) });
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

        private int _roomSize;
        [Range(0, 500,ErrorMessage ="取值范围为0-500")]
        public int RoomSize
        {
            get => _roomSize;
            set => Set(ref _roomSize, value);
        }
    }
}
