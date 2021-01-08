using DataModel;
using GalaSoft.MvvmLight;
using System;
using System.ComponentModel.DataAnnotations;

namespace AIForAgedClient
{
    public class PoseInfoVM : ViewModelBase
    {
        public long AgesInfoId { get; set; }
        public AgesInfo AgesInfo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        private int _timeStand;

        public int TimeStand
        {
            get => _timeStand;
            set => Set(() => TimeStand, ref _timeStand, value);
        }

        private int _timeSit;

        public int TimeSit
        {
            get => _timeSit;
            set => Set(() => TimeSit, ref _timeSit, value);
        }

        private int _timeLie;

        public int TimeLie
        {
            get => _timeLie;
            set => Set(() => TimeLie, ref _timeLie, value);
        }

        private int _timeDown;

        public int TimeDown
        {
            get => _timeDown;
            set => Set(() => TimeDown, ref _timeDown, value);
        }

        private int _timeOther;

        public int TimeOther
        {
            get => _timeOther;
            set => Set(() => TimeOther, ref _timeOther, value);
        }

        private string _timeIn;

        [StringLength(8)]
        public string TimeIn
        {
            get => _timeIn;
            set => Set(() => TimeIn, ref _timeIn, value);
        }

        private bool _isAlarm;

        public bool IsAlarm
        {
            get => _isAlarm;
            set => Set(() => IsAlarm, ref _isAlarm, value);
        }

        private byte? _status;

        public byte? Status
        {
            get => _status;
            set => Set(() => Status, ref _status, value);
        }
    }
}