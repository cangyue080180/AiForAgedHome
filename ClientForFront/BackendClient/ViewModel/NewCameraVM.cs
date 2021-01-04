using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BackendClient.ViewModel
{
    public class NewCameraVM : NewModelVMBase<CameraInfoVM>, INotifyPropertyChanged
    {
        private readonly bool isNew = true;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;

        public ObservableCollection<RoomInfoVM> Rooms { get; } = new ObservableCollection<RoomInfoVM>();
        public ObservableCollection<ServerInfoVM> ServerInfoes { get; } = new ObservableCollection<ServerInfoVM>();

        public RoomInfoVM SelectedRoom { get; set; }
        public ServerInfoVM SelectedServerInfo { get; set; }

        public NewCameraVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.httpClient = httpClient;
            this.autoMapper = autoMapper;

            if (SimpleIoc.Default.IsRegistered<CameraInfoVM>())//修改
            {
                isNew = false;
                Model = SimpleIoc.Default.GetInstance<CameraInfoVM>();
                Title = "修改摄像头信息";
                Rooms.Add(Model.RoomInfo);
                ServerInfoes.Add(Model.ServerInfo);
                SelectedRoom = Model.RoomInfo;
                SelectedServerInfo = Model.ServerInfo;
            }
            else
            {
                isNew = true;
                Model = new CameraInfoVM();
                Title = "创建新摄像头";
            }
        }

        private RelayCommand _testVideoUrlCmd;

        public RelayCommand TestVideoUrlCmd
        {
            get
            {
                if (_testVideoUrlCmd == null)
                    _testVideoUrlCmd = new RelayCommand(TestVideoUrl);
                return _testVideoUrlCmd;
            }
        }

        private BitmapSource _img1;

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapSource Image1
        {
            get
            {
                return _img1;
            }
            set
            {
                if (_img1 != value)
                {
                    _img1 = value;
                    RaisePropertyChanged(nameof(Image1));
                }
            }
        }

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override bool IsNewModel()
        {
            return isNew;
        }

        private double bitmapWidth = 1920, bitmapHeight = 1080;
        private double _height = 300;

        public double Height
        {
            get => _height;
            set
            {
                if (value != _height)
                {
                    _height = value;

                    RaisePropertyChanged(nameof(Height));
                    RegionTop = Model.LeftTopPointY * Height / (bitmapHeight / 2);
                    RegionLeft = Model.LeftTopPointX * Width / (bitmapWidth / 2);
                    RegionWidth = (Model.RightBottomPointX - Model.LeftTopPointX) * Width / (bitmapWidth / 2);
                    RegionHeight = (Model.RightBottomPointY - Model.LeftTopPointY) * Height / (bitmapHeight / 2);
                }
            }
        }

        public double Width => 400;

        private double _regionTop = 100;

        public double RegionTop
        {
            get => _regionTop;
            set
            {
                if (_regionTop != value)
                {
                    _regionTop = value;
                    RaisePropertyChanged(nameof(RegionTop));
                }
            }
        }

        private double _regionLeft = 100;

        public double RegionLeft
        {
            get => _regionLeft;
            set
            {
                if (_regionLeft != value)
                {
                    _regionLeft = value;
                    RaisePropertyChanged(nameof(RegionLeft));
                }
            }
        }

        private double _regionWidth = 20;

        public double RegionWidth
        {
            get => _regionWidth;
            set
            {
                if (_regionWidth != value)
                {
                    _regionWidth = value;
                    RaisePropertyChanged(nameof(RegionWidth));
                }
            }
        }

        private double _regionHeight = 20;

        public double RegionHeight
        {
            get => _regionHeight;
            set
            {
                if (_regionHeight != value)
                {
                    _regionHeight = value;
                    RaisePropertyChanged(nameof(RegionHeight));
                }
            }
        }

        private VideoPlayHelper playHelper;

        private void TestVideoUrl()
        {
            playHelper?.Stop();
            playHelper = new VideoPlayHelper(Model.VideoAddress, (x) =>
            {
                Image1 = x;
                if (x != null)
                {
                    //此时读到的x.Width和x.Height为原始未压缩尺寸，实际处理过程中图像宽和高都被压缩了1/2
                    bitmapHeight = x.Height;
                    bitmapWidth = x.Width;
                    Height = x.Height * 400 / x.Width;
                }
            });
            playHelper.Start();
        }

        public async override Task<bool> Ok()
        {
            Model.LeftTopPointY = (int)(_regionTop * (bitmapHeight / 2) / Height);
            Model.LeftTopPointX = (int)(_regionLeft * (bitmapWidth / 2) / Width);
            Model.RightBottomPointX = (int)((_regionWidth + _regionLeft) * (bitmapWidth / 2) / Width);
            Model.RightBottomPointY = (int)((_regionHeight + _regionTop) * (bitmapHeight / 2) / Height);

            if (isNew)
            {
                Model.RoomInfoId = SelectedRoom.Id;
                Model.ServerInfoId = SelectedServerInfo.Id;
                return await Common.PostNew(httpClient, ConfigurationManager.AppSettings["GetCameraInfoUrl"], autoMapper.Map<CameraInfo>(Model));
            }
            else
            {
                Model.RoomInfoId = SelectedRoom.Id;
                Model.ServerInfoId = SelectedServerInfo.Id;
                Model.RoomInfo = SelectedRoom;
                Model.ServerInfo = SelectedServerInfo;
                return await Common.Put(httpClient, ConfigurationManager.AppSettings["GetCameraInfoUrl"], Model.Id, autoMapper.Map<CameraInfo>(Model));
            }
        }

        public override void OnWindowClosing()
        {
            LogHelper.Debug("OnNewAgedWindowClosing()");

            playHelper?.Stop();

            if (!isNew)
            {
                SimpleIoc.Default.Unregister<CameraInfoVM>();
            }
        }

        public override void OnWindowLoaded()
        {
            LogHelper.Debug("OnNewAgedWindowLoaded()");
            UpdateRoomsAsync(Rooms);
            UpdateServerInfoesAsync(ServerInfoes);
        }

        private async void UpdateServerInfoesAsync(IList<ServerInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetServerInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetServerInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var sourceCollection = JsonConvert.DeserializeObject<List<ServerInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<ServerInfoVM>(item));
                    }
                    else
                    {
                        autoMapper.Map(item, exitInfo);
                    }
                }
                //检查有无删减
                for (int i = targetCollection.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in sourceCollection)
                    {
                        if (targetCollection.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        targetCollection.RemoveAt(i);
                    }
                }
            }
        }

        private async void UpdateRoomsAsync(IList<RoomInfoVM> targetCollection)
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetAgesInfoes caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var sourceCollection = JsonConvert.DeserializeObject<List<RoomInfo>>(result);
                //检查有无新增
                foreach (var item in sourceCollection)
                {
                    var exitInfo = targetCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (exitInfo == null)
                    {
                        targetCollection.Add(autoMapper.Map<RoomInfoVM>(item));
                    }
                    else
                    {
                        autoMapper.Map(item, exitInfo);
                    }
                }
                //检查有无删减
                for (int i = targetCollection.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in sourceCollection)
                    {
                        if (targetCollection.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        targetCollection.RemoveAt(i);
                    }
                }
            }
        }
    }
}