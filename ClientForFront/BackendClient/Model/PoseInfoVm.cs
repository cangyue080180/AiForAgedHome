using GalaSoft.MvvmLight;

namespace BackendClient.Model
{
    public class PoseInfoVm : ViewModelBase
    {
        private double _timeStand;
        public double TimeStand
        {
            get => _timeStand;
            set => Set(ref _timeStand, value);
        }

        private double _timeSit = 0;
        public double TimeSit
        {
            get => _timeSit;

            set => Set(ref _timeSit, value);
        }

        private double _timeLie = 0;
        public double TimeLie
        {
            get => _timeLie;

            set => Set(ref _timeLie, value);
        }

        private double _timeDown = 0;
        public double TimeDown
        {
            get => _timeDown;

            set => Set(ref _timeDown, value);
        }

        private double _timeOther = 0;
        public double TimeOther
        {
            get => _timeOther;

            set => Set(ref _timeOther, value);
        }
    }
}
