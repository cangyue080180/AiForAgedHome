using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class DataViewVM : ViewModelBase
    {
        #region property and command
        private HttpClient httpClient;
        private Timer updateTimer;

        public PoseInfoVm PoseInfoVm_Now { get; set; }//当天的pose信息
        public PoseInfoVm PoseInfoVm_Week { get; set; }//最近一周的pose信息
        public PoseInfoVm PoseInfoVm_Month { get; set; }//最近一月的pose信息

        private ObservableCollection<AgesInfo> _ageds = new ObservableCollection<AgesInfo>();
        public ObservableCollection<AgesInfo> Ageds
        {
            get
            {
                return _ageds;
            }
        }

        private AgesInfo _selectedAged;
        public AgesInfo SelectedAged
        {
            get => _selectedAged;
            set => Set(ref _selectedAged,value);
        }

        private RelayCommand _onLoadedCmd;
        public ICommand OnLoadedCmd
        {
            get
            {
                if (_onLoadedCmd == null)
                    _onLoadedCmd = new RelayCommand(Loaded);
                return _onLoadedCmd;
            }
        }

        private RelayCommand _onUnloadedCmd;
        public ICommand OnUnloadedCmd
        {
            get
            {
                if (_onUnloadedCmd == null)
                    _onUnloadedCmd = new RelayCommand(Unloaded);
                return _onUnloadedCmd;
            }
        }

        private RelayCommand _userSelectedChangedCmd;
        public ICommand UserSelectionChangeCmd
        {
            get
            {
                if (_userSelectedChangedCmd == null)
                    _userSelectedChangedCmd = new RelayCommand(OnUserSelectionChangedAsync);
                return _userSelectedChangedCmd;
            }
        }
        #endregion

        public DataViewVM()
        {
            //初始化
            httpClient = MainViewModel.httpClient;
            updateTimer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            PoseInfoVm_Now = new PoseInfoVm();
            PoseInfoVm_Week = new PoseInfoVm();
            PoseInfoVm_Month = new PoseInfoVm();

            GetAgedsAsync();
        }

        private void Loaded()
        {
            LogHelper.Debug("DataView Loaded.");
            GetAgedsAsync();
        }

        private void TimerCallback(object state)
        {
            //从数据库中定时更新数据
            //string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];
            //url += $"/{SelectedAged.Id}";

           // updateTimer.Change(2000,Timeout.Infinite);
        }

        //从数据库加载老人信息
        private async void GetAgedsAsync()
        {
            string url = ConfigurationManager.AppSettings["GetAgedsUrl"];
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
                var ageds = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
                //检查有无新增
                foreach (var item in ageds)
                {
                    if (!Ageds.Any(x=>x.Id==item.Id))
                    {
                        Ageds.Add(item);
                    }
                }
                //检查有无删减
                for(int i = Ageds.Count-1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach(var item in ageds)
                    {
                        if (Ageds.ElementAt(i).Id == item.Id)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        Ageds.RemoveAt(i);
                    }
                }
            }
        }

        //选择用户改变事件
        private async void OnUserSelectionChangedAsync()
        {
            LogHelper.Debug($"selected aged: {SelectedAged.Name}");

            await GetPoseInfoOfMonthAsync(SelectedAged.Id);
        }

        private async Task GetPoseInfoOfMonthAsync(long agedId)
        {
            //获取选择用户最近一个月的姿态信息
            string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];
            url += $"/GetPoseInfoDays?id={agedId}&minDate={DateTime.Now.AddDays(-29).Date}&maxDate={DateTime.Now.AddDays(1).Date}";
            string result;

            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                result = null;
                LogHelper.Debug($"get poseInfo caught exception: {e.Message}");
            }

            if (!string.IsNullOrEmpty(result))
            {
                List<PoseInfo> poseinfoes = JsonConvert.DeserializeObject<List<PoseInfo>>(result);

                //更新当前的姿态信息
                var poseinfo = poseinfoes.FirstOrDefault(x => x.Date == DateTime.Now.Date);
                if (poseinfo != null)
                {
                    PoseInfoVm_Now.TimeDown = poseinfo.TimeDown;
                    PoseInfoVm_Now.TimeSit = poseinfo.TimeSit;
                    PoseInfoVm_Now.TimeLie = poseinfo.TimeLie;
                    PoseInfoVm_Now.TimeOther = poseinfo.TimeOther;
                    PoseInfoVm_Now.TimeStand = poseinfo.TimeStand;
                }
                //更新最近一周的姿态信息
                var poseInfoWeek = poseinfoes.Where(x => x.Date >= DateTime.Now.AddDays(-6).Date && x.Date <= DateTime.Now);
                foreach (var item in poseInfoWeek)
                {
                    PoseInfoVm_Week.TimeDown += item.TimeDown;
                    PoseInfoVm_Week.TimeLie += item.TimeLie;
                    PoseInfoVm_Week.TimeOther += item.TimeOther;
                    PoseInfoVm_Week.TimeSit += item.TimeSit;
                    PoseInfoVm_Week.TimeStand += item.TimeStand;
                }
                //更新最近一月的姿态信息
                foreach (var item in poseinfoes)
                {
                    PoseInfoVm_Month.TimeDown += item.TimeDown;
                    PoseInfoVm_Month.TimeLie += item.TimeLie;
                    PoseInfoVm_Month.TimeOther += item.TimeOther;
                    PoseInfoVm_Month.TimeSit += item.TimeSit;
                    PoseInfoVm_Month.TimeStand += item.TimeStand;
                }
            }
        } 

        private void Unloaded()
        {
            LogHelper.Debug("DataView Unloaded.");
        }
    }
}
