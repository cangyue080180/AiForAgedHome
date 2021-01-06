using DataModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIForAgedClient.Model
{
    public class RoomInfoVM : ViewModelBase
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [Range(0, 500)]
        public int RoomSize { get; set; }

        private bool _isAlarm;

        //是否警报
        public bool IsAlarm
        {
            get
            {
                return _isAlarm;
            }
            set
            {
                Set(() => IsAlarm, ref _isAlarm, value);
            }
        }

        public List<AgesInfo> AgesInfos { get; set; }

        public List<CameraInfo> CameraInfos { get; set; }
    }
}