using AIForAgedClient.Helper;
using AIForAgedClient.View;
using AutoMapper;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AIForAgedClient.ViewModel
{
    public class ManyPersonMonitorVM : ViewModelBase
    {
        private DispatcherTimer dispatcherTimer;
        private HttpClient httpClient;
        private Mapper mapper;

        public ObservableCollection<PoseInfoVM> PoseInfos { get; } = new ObservableCollection<PoseInfoVM>();

        private PoseInfoVM _selectedPoseInfo;

        public PoseInfoVM SelectedPoseInfo
        {
            get => _selectedPoseInfo;
            set => Set(ref _selectedPoseInfo, value);
        }

        public BaseFourVideoVM FourVideoVM { get; }
        public RoomInfo RoomInfo { get; }

        private RelayCommand _onLoaded;

        public RelayCommand OnLoadedCommand
        {
            get
            {
                if (_onLoaded == null)
                    _onLoaded = new RelayCommand(OnWindowLoaded);
                return _onLoaded;
            }
        }

        private RelayCommand _onClosing;

        public RelayCommand OnClosingCommand
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

        private RelayCommand<Button> _goDetailViewCmd;

        public RelayCommand<Button> GoDetailViewCmd
        {
            get
            {
                if (_goDetailViewCmd == null)
                {
                    _goDetailViewCmd = new RelayCommand<Button>(btn =>
                    {
                        var selectedItem = btn.DataContext as PoseInfoVM;
                        this.SelectedPoseInfo = selectedItem;
                        SimpleIoc.Default.Register(() => SelectedPoseInfo);

                        ShowDetailPoseInfoWindow();
                    });
                }
                return _goDetailViewCmd;
            }
        }

        public ManyPersonMonitorVM(RoomInfo roomInfo, HttpClient httpClient, BaseFourVideoVM fourVideoViewModel, Mapper mapper)
        {
            this.RoomInfo = roomInfo;
            this.httpClient = httpClient;
            this.FourVideoVM = fourVideoViewModel;
            this.mapper = mapper;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetPosesAsync();
        }

        private async Task GetPosesAsync()
        {
            string url = ConfigurationManager.AppSettings["GetPoseInfoByRoomUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url + "?id=" + RoomInfo.Id);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetAgeds caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var datas = JsonConvert.DeserializeObject<List<PoseInfo>>(result);
                UpdateDataSource(datas);
            }
        }

        private void UpdateDataSource(IEnumerable<PoseInfo> poseInfos)
        {
            //检查有无新增
            foreach (var item in poseInfos)
            {
                var tempPose = PoseInfos.FirstOrDefault(x => x.AgesInfoId == item.AgesInfoId);
                if (tempPose == null)
                {
                    PoseInfos.Add(mapper.Map<PoseInfoVM>(item));
                }
                else
                {
                    mapper.Map(item, tempPose);
                }
            }
            //检查有无删减
            for (int i = PoseInfos.Count - 1; i >= 0; i--)
            {
                bool isExit = false;
                foreach (var item in poseInfos)
                {
                    if (PoseInfos.ElementAt(i).AgesInfoId == item.AgesInfoId)
                    {
                        isExit = true;
                        break;
                    }
                }

                if (!isExit)
                {
                    PoseInfos.RemoveAt(i);
                }
            }
        }

        private async void OnWindowLoaded()
        {
            Console.WriteLine("ManyPersonWindowLoaded");
            await GetPosesAsync();
            dispatcherTimer.Start();

            if (this.FourVideoVM is FourVideoViewModel)
            {
                if (RoomInfo.CameraInfos.Count > 0)
                {
                    FourVideoVM.Url1 = RoomInfo.CameraInfos[0].VideoAddress;
                    if (RoomInfo.CameraInfos.Count > 1)
                        FourVideoVM.Url2 = RoomInfo.CameraInfos[1].VideoAddress;
                    if (RoomInfo.CameraInfos.Count > 2)
                        FourVideoVM.Url3 = RoomInfo.CameraInfos[2].VideoAddress;
                    if (RoomInfo.CameraInfos.Count > 3)
                        FourVideoVM.Url4 = RoomInfo.CameraInfos[3].VideoAddress;
                }
            }
            else if (this.FourVideoVM is HuoChaiRenFourVideoVM)
            {
                HuoChaiRenFourVideoVM huoChaiRenFourVideoVM = FourVideoVM as HuoChaiRenFourVideoVM;
                huoChaiRenFourVideoVM.RoomId = (uint)RoomInfo.Id;
                if (RoomInfo.CameraInfos.Count > 0)
                {
                    huoChaiRenFourVideoVM.Url1 = RoomInfo.CameraInfos[0].Id.ToString();
                    if (RoomInfo.CameraInfos.Count > 1)
                        huoChaiRenFourVideoVM.Url2 = RoomInfo.CameraInfos[1].Id.ToString();
                    if (RoomInfo.CameraInfos.Count > 2)
                        huoChaiRenFourVideoVM.Url3 = RoomInfo.CameraInfos[2].Id.ToString();
                    if (RoomInfo.CameraInfos.Count > 3)
                        huoChaiRenFourVideoVM.Url4 = RoomInfo.CameraInfos[3].Id.ToString();
                }
            }
            else if (this.FourVideoVM is HuoChaiAndOriginVideoVM)
            {
                HuoChaiAndOriginVideoVM huoChaiAndOriginVideoVM = FourVideoVM as HuoChaiAndOriginVideoVM;
                huoChaiAndOriginVideoVM.RoomId = (uint)RoomInfo.Id;
                if (RoomInfo.CameraInfos.Count == 1)
                {
                    huoChaiAndOriginVideoVM.Url1 = RoomInfo.CameraInfos[0].Id.ToString();
                    huoChaiAndOriginVideoVM.Url2 = RoomInfo.CameraInfos[0].VideoAddress;
                }
                else
                {
                    System.Console.WriteLine("当前不支持一个房间多个摄像头");
                }
            }
            else
            {
            }

            FourVideoVM.Start();
        }

        private void ShowDetailPoseInfoWindow()
        {
            DetailPoseInfoWindow tempWindow = new DetailPoseInfoWindow();
            //tempWindow.Owner = App.Current.MainWindow;
            tempWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            tempWindow.ShowDialog();
        }

        private void OnWindowClosing()
        {
            Console.WriteLine("ManyPersonWindowClosed");
            dispatcherTimer.Stop();

            SimpleIoc.Default.Unregister<RoomInfo>();
            SimpleIoc.Default.Unregister<BaseFourVideoVM>();
        }
    }
}