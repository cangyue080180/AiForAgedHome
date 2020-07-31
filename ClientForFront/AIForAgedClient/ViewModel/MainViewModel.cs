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
using System.Threading;
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
        private DispatcherTimer dispatcherTimer;

        public ObservableCollection<PoseInfo> PoseInfos { get; } = new ObservableCollection<PoseInfo>();

        private PoseInfo _selectedPoseInfo;
        public PoseInfo SelectedPoseInfo
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
                        var selectedItem = x.DataContext as PoseInfo;
                        this.SelectedPoseInfo = selectedItem;
                        SimpleIoc.Default.Register(() => SelectedPoseInfo);

                        MonitorWindow monitorWindow = new MonitorWindow();
                        monitorWindow.Owner = App.Current.MainWindow;
                        monitorWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                        monitorWindow.ShowDialog();
                    });
                }
                return _goMonitorViewCmd;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetAgedsAsync();
        }

        private async void OnWindowLoaded()
        {
            await GetAgedsAsync();
            dispatcherTimer.Start();
        }

        private void OnWindowClosing()
        {
            dispatcherTimer.Stop();
        }

        private async Task GetAgedsAsync()
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
                var ageds = JsonConvert.DeserializeObject<List<PoseInfo>>(result);
                //检查有无新增
                foreach (var item in ageds)
                {
                    if (!PoseInfos.Any(x => x.AgesInfoId == item.AgesInfoId))
                    {
                        PoseInfos.Add(item);
                    }
                }
                //检查有无删减
                for (int i = PoseInfos.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in ageds)
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
        }
    }
}