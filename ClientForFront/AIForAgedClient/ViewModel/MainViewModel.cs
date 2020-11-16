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
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AIForAgedClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private HttpClient httpClient;
        private IMapper mapper;
        private DispatcherTimer dispatcherTimer;

        public ObservableCollection<PoseInfoVM> PoseInfos { get; } = new ObservableCollection<PoseInfoVM>();

        private PoseInfoVM _selectedPoseInfo;
        public PoseInfoVM SelectedPoseInfo
        {
            get => _selectedPoseInfo;
            set => Set(ref _selectedPoseInfo, value);
        }

        public string WindowName
        {
            get => Assembly.GetExecutingAssembly().GetName().Name;
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

        private RelayCommand<Button> _goMonitorViewCmd;
        public ICommand GoMonitorViewCmd
        {
            get
            {
                if (_goMonitorViewCmd == null)
                {
                    _goMonitorViewCmd = new RelayCommand<Button>((x) =>
                    {
                        var selectedItem = x.DataContext as PoseInfoVM;
                        this.SelectedPoseInfo = selectedItem;
                        SimpleIoc.Default.Register(() => SelectedPoseInfo);

                        string video_type = ConfigurationManager.AppSettings["video_type"].Trim();
                        if (video_type == "orignal")
                            SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();
                        else if (video_type == "huo_chai_ren")
                            SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiRenFourVideoVM>();
                        else if(video_type == "huochai_and_origin")
                            SimpleIoc.Default.Register<BaseFourVideoVM, HuoChaiAndOriginVideoVM>();
                        else
                            SimpleIoc.Default.Register<BaseFourVideoVM, FourVideoViewModel>();

                        ShowMonitorWindow();
                    });
                }
                return _goMonitorViewCmd;
            }
        }

        private RelayCommand<Button> _goDetailViewCmd;
        public ICommand GoDetailViewCmd
        {
            get
            {
                if (_goDetailViewCmd == null)
                {
                    _goDetailViewCmd = new RelayCommand<Button>(btn=> {
                        var selectedItem = btn.DataContext as PoseInfoVM;
                        this.SelectedPoseInfo = selectedItem;
                        SimpleIoc.Default.Register(() => SelectedPoseInfo);

                        ShowDetailPoseInfoWindow();
                    });
                }
                return _goDetailViewCmd;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(HttpClient httpClient,Mapper mapper)
        {
            this.httpClient = httpClient;
            this.mapper = mapper;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetPosesAsync();
        }

        private async void OnWindowLoaded()
        {
            await GetPosesAsync();
            dispatcherTimer.Start();
        }

        private void OnWindowClosing()
        {
            dispatcherTimer.Stop();
        }

        private async Task GetPosesAsync()
        {
            string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];
            string result;
            try
            {
                result = await httpClient.GetStringAsync(url);
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
                    mapper.Map(item,tempPose);
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

        private void ShowMonitorWindow()
        {
            MonitorWindow monitorWindow = new MonitorWindow();
            monitorWindow.Owner = App.Current.MainWindow;
            monitorWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            monitorWindow.ShowDialog();
        }
        private void ShowDetailPoseInfoWindow()
        {
            DetailPoseInfoWindow tempWindow = new DetailPoseInfoWindow();
            tempWindow.Owner = App.Current.MainWindow;
            tempWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            tempWindow.ShowDialog();
        }
    }
}