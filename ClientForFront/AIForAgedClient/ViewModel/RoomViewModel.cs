using AIForAgedClient.Helper;
using AIForAgedClient.View;
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
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AIForAgedClient.ViewModel
{
    public class RoomViewModel:ViewModelBase
    {
        #region variables and properies
        private HttpClient httpClient;
        private DispatcherTimer dispatcherTimer;

        public ObservableCollection<RoomInfo> RoomInfoes { get; } = new ObservableCollection<RoomInfo>();

        private RoomInfo _selectedRoom;
        public RoomInfo SelectedRoom
        {
            get => _selectedRoom;
            set => Set(ref _selectedRoom, value);
        }

        public string WindowName
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var titleAttribute = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly,typeof(AssemblyTitleAttribute));
                return titleAttribute.Title;
            }
        }

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

        private RelayCommand<Button> _goMonitorViewCmd;
        public RelayCommand<Button> GoMonitorViewCmd
        {
            get
            {
                if (_goMonitorViewCmd == null)
                {
                    _goMonitorViewCmd = new RelayCommand<Button>((x) =>
                    {
                        var selectedItem = x.DataContext as RoomInfo;
                        this.SelectedRoom = selectedItem;
                        SimpleIoc.Default.Register(() => SelectedRoom);

                        string video_type = ConfigurationManager.AppSettings["video_type"].Trim();
                        if (video_type == "orignal")
                            SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
                        else if (video_type == "huo_chai_ren")
                            SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiRenFourVideoVM>();
                        else if (video_type == "huochai_and_origin")
                            SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiAndOriginVideoVM>();
                        else
                            SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();

                        ShowMonitorWindow();
                    });
                }
                return _goMonitorViewCmd;
            }
        }
        #endregion

        public RoomViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetRoomsAsync();
        }
        private async Task GetRoomsAsync()
        {
            string url = ConfigurationManager.AppSettings["GetRoomInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                LogHelper.Debug($"GetRooms caught exception: {e.Message}");
                result = null;
            }

            if (!string.IsNullOrEmpty(result))
            {
                var datas = JsonConvert.DeserializeObject<List<RoomInfo>>(result);
                UpdateDataSource(datas);
            }
        }

        private void UpdateDataSource(IEnumerable<RoomInfo> roomInfoes)
        {
            //检查有无新增
            foreach (var item in roomInfoes)
            {
                var tempPose = RoomInfoes.FirstOrDefault(x => x.Id == item.Id);
                if (tempPose == null)
                {
                    RoomInfoes.Add(item);
                }
                else
                {
                    //更新状态信息
                    tempPose.IsAlarm = item.IsAlarm;
                }
                
            }
            //检查有无删减
            for (int i = RoomInfoes.Count - 1; i >= 0; i--)
            {
                bool isExit = false;
                foreach (var item in roomInfoes)
                {
                    if (RoomInfoes.ElementAt(i).Id == item.Id)
                    {
                        isExit = true;
                        break;
                    }
                }

                if (!isExit)
                {
                    RoomInfoes.RemoveAt(i);
                }
            }
        }

        private void ShowMonitorWindow()
        {
            ManyPersonMonitorWindow monitorWindow = new ManyPersonMonitorWindow();
            monitorWindow.Owner = App.Current.MainWindow;
            monitorWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            monitorWindow.ShowDialog();
        }

        private void OnWindowLoaded()
        {
            Console.WriteLine("RoomWindow loaded");

            GetRoomsAsync();
            dispatcherTimer.Start();
        }

        private void OnWindowClosing()
        {
            Console.WriteLine("RoomWindow Closing");
            dispatcherTimer.Stop();
        }
    }
}
