using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
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

        private List<AgesInfo> _ageds = new List<AgesInfo>();
        public List<AgesInfo> Ageds
        {
            get
            {
                return _ageds;
            }
            set
            {
                if (value != _ageds)
                {
                    _ageds = value;
                    RaisePropertyChanged(nameof(Ageds));
                }
            }
        }

        public AgesInfo SelectedAged
        {
            get;set;
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
                    _userSelectedChangedCmd = new RelayCommand(OnUserSelectionChanged);
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
        }

        private void Loaded()
        {
            LogHelper.Debug("DataView Loaded.");

            PoseInfoVm_Now = new PoseInfoVm();
            PoseInfoVm_Week = new PoseInfoVm();
            PoseInfoVm_Month = new PoseInfoVm();
            GetAgedsAsync();
        }

        private void TimerCallback(object state)
        {
            //从数据库中定时更新数据
            string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];
            url += $"/{SelectedAged.Id}";

            updateTimer.Change(1,Timeout.Infinite);
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
                Ageds = JsonConvert.DeserializeObject<List<AgesInfo>>(result);
            }
        }

        //选择用户改变事件
        private async void OnUserSelectionChanged()
        {
            LogHelper.Debug($"selected aged: {SelectedAged.Name}");

            //获取选择用户最近一个月的姿态信息
            string url = ConfigurationManager.AppSettings["GetPoseInfoUrl"];
            url += $"/GetPoseInfoDays?id={SelectedAged.Id}&minDate={DateTime.Now.AddDays(-29).Date}&maxDate={DateTime.Now.AddDays(1).Date}";
            string result;

            try
            {
                result =await httpClient.GetStringAsync(url);
            }catch(HttpRequestException e)
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
                foreach(var item in poseInfoWeek)
                {
                    PoseInfoVm_Week.TimeDown += item.TimeDown;
                    PoseInfoVm_Week.TimeLie += item.TimeLie;
                    PoseInfoVm_Week.TimeOther += item.TimeOther;
                    PoseInfoVm_Week.TimeSit += item.TimeSit;
                    PoseInfoVm_Week.TimeStand += item.TimeStand;
                }
                //更新最近一月的姿态信息
                foreach(var item in poseinfoes)
                {
                    PoseInfoVm_Month.TimeDown += item.TimeDown;
                    PoseInfoVm_Month.TimeLie += item.TimeLie;
                    PoseInfoVm_Month.TimeOther += item.TimeOther;
                    PoseInfoVm_Month.TimeSit += item.TimeSit;
                    PoseInfoVm_Month.TimeStand += item.TimeStand;
                }
            }

           // updateTimer.Change(Timeout.Infinite,Timeout.Infinite);
        }

        private void Unloaded()
        {
            LogHelper.Debug("DataView Unloaded.");
        }
    }
}
