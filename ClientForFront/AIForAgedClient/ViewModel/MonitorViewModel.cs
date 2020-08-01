using AIForAgedClient.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public class MonitorViewModel : ViewModelBase
    {
        private FourVideoViewModel fourVideoViewModel;
        public FourVideoViewModel FourVideoVM
        {
            get => fourVideoViewModel;
        }

        private PoseInfoVM _poseInfo;
        public PoseInfoVM PoseInfo
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
        public MonitorViewModel(PoseInfoVM poseInfo, FourVideoViewModel fourVideoViewModel)
        {
            this._poseInfo = poseInfo;
            this.fourVideoViewModel = fourVideoViewModel;
        }

        private void OnWindowLoaded()
        {
            if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 0)
            {
                FourVideoVM.Url1 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[0].VideoAddress;
                FourVideoVM.Url2 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[1].VideoAddress;
                FourVideoVM.Url3 = null;
                FourVideoVM.Url4 = null;
            }
            FourVideoVM.Start();
        }

        private void OnWindowClosing()
        {
            LogHelper.Debug(nameof(MonitorViewModel) + " Closing");
            FourVideoVM.Stop();
            SimpleIoc.Default.Unregister<PoseInfoVM>();
        }
    }
}
