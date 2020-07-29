using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public class MonitorViewModel:ViewModelBase
    {
        private FourVideoViewModel fourVideoViewModel;
        public FourVideoViewModel FourVideoVM
        {
            get => fourVideoViewModel;
        }

        private PoseInfo _poseInfo;
        public PoseInfo PoseInfo
        {
            get => _poseInfo;
        }

        private RelayCommand _onLoaded;
        public ICommand OnLoadedCommand
        {
            get
            {
                if (_onLoaded == null)
                    _onLoaded = new RelayCommand(OnWindowLoaded);
                return _onLoaded;
            }
        }

        private RelayCommand _onClosing;
        public ICommand OnClosingCommand
        {
            get
            {
                if (_onClosing == null)
                {
                    _onClosing = new RelayCommand(OnWindowClosing);
                }
                return _onClosing;
            }
        }
        public MonitorViewModel(PoseInfo poseInfo)
        {
            this._poseInfo=poseInfo;
            fourVideoViewModel = new FourVideoViewModel();
        }

        private void OnWindowLoaded()
        {
            FourVideoVM.Start();
        }

        private void OnWindowClosing()
        {
            FourVideoVM.Stop();
        }
    }
}
