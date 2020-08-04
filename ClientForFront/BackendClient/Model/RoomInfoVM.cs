using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendClient.Model
{
    public class RoomInfoVM : ViewModelBase
    {
        public long Id { get; set; }

        private string _name;
        [StringLength(20)]
        public string Name {
            get => _name;
            set => Set(ref _name,value);
        }

        private int _roomSize;
        [Range(0, 500)]
        public int RoomSize {
            get => _roomSize;
            set => Set(ref _roomSize,value);
        }
    }
}
