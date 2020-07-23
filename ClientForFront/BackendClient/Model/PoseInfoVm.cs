using DataModel;
using GalaSoft.MvvmLight;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendClient.Model
{
    public class PoseInfoVm:ViewModelBase
    {
        private int _timeStand;
        public int TimeStand {
            get => _timeStand;
            set => Set(ref _timeStand,value);
        }

        private int _timeSit=0;
        public int TimeSit {
            get => _timeSit;

            set => Set(ref _timeSit,value);
        }

        private int _timeLie=0;
        public int TimeLie {
            get => _timeLie;

            set => Set(ref _timeLie,value);
        }

        private int _timeDown=0;
        public int TimeDown {
            get => _timeDown;

            set => Set(ref _timeDown,value);
        }

        private int _timeOther=0;
        public int TimeOther {
            get => _timeOther;

            set => Set(ref _timeOther,value);
        }
    }
}
