using AIForAgedClient.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Input;

namespace AIForAgedClient.ViewModel
{
    public class MonitorViewModel : ViewModelBase
    {
        private BaseFourVideoVM fourVideoViewModel;
        public BaseFourVideoVM FourVideoVM
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
        public MonitorViewModel(PoseInfoVM poseInfo, BaseFourVideoVM fourVideoViewModel)
        {
            this._poseInfo = poseInfo;
            this.fourVideoViewModel = fourVideoViewModel;
        }

        private void OnWindowLoaded()
        {
            if (this.fourVideoViewModel is FourVideoViewModel)
            {
                if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 0)
                {
                    FourVideoVM.Url1 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[0].VideoAddress;
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 1)
                        FourVideoVM.Url2 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[1].VideoAddress;
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 2)
                        FourVideoVM.Url3 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[2].VideoAddress;
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 3)
                        FourVideoVM.Url4 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[3].VideoAddress;
                }
            }else if (this.fourVideoViewModel is HuoChaiRenFourVideoVM)
            {
                HuoChaiRenFourVideoVM huoChaiRenFourVideoVM = fourVideoViewModel as HuoChaiRenFourVideoVM;
                huoChaiRenFourVideoVM.RoomId = (uint)PoseInfo.AgesInfo.RoomInfoId;
                if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 0)
                {
                    huoChaiRenFourVideoVM.Url1 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[0].Id.ToString();
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 1)
                        huoChaiRenFourVideoVM.Url2 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[1].Id.ToString();
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 2)
                        huoChaiRenFourVideoVM.Url3 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[2].Id.ToString();
                    if (PoseInfo.AgesInfo.RoomInfo.CameraInfos.Count > 3)
                        huoChaiRenFourVideoVM.Url4 = PoseInfo.AgesInfo.RoomInfo.CameraInfos[3].Id.ToString();
                }
            }
            else
            {

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
