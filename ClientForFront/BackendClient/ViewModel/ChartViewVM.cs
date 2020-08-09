using AutoMapper;
using BackendClient.Model;
using DataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LiveCharts;
using LiveCharts.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackendClient.ViewModel
{
    public class ChartViewVM : ViewModelBase
    {
        #region property and command
        private HttpClient httpClient;
        private IMapper autoMapper;
        private Timer updateTimer;

        public PoseInfoVm PoseInfoVm_Now { get; set; }//当天的pose信息
        public PoseInfoVm PoseInfoVm_Week { get; set; }//最近一周的pose信息
        public PoseInfoVm PoseInfoVm_Month { get; set; }//最近一月的pose信息
        public ChartValues<MeasureModel> PoseInfoStandWeek { get; set; }
        public ChartValues<MeasureModel> PoseInfoSitWeek { get; set; }
        public ChartValues<MeasureModel> PoseInfoLieWeek { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }

        public double AxisMax { get; set; }
        public double AxisMin { get; set; }

        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

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
            set => Set(ref _selectedAged, value);
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

        public ChartViewVM(HttpClient httpClient, Mapper autoMapper)
        {
            this.autoMapper = autoMapper;

            var mapper = Mappers.Xy<MeasureModel>()
               .X(model => model.DateTime.Ticks)
               .Y(model => model.Value);

            Charting.For<MeasureModel>(mapper);

            PoseInfoStandWeek = new ChartValues<MeasureModel>();
            PoseInfoLieWeek = new ChartValues<MeasureModel>();
            PoseInfoSitWeek = new ChartValues<MeasureModel>();
            AxisMax = DateTime.Now.Ticks;
            AxisMin = DateTime.Now.Ticks - TimeSpan.FromDays(7).Ticks;
            DateTimeFormatter = value => new DateTime((long)value).ToString("MM/dd");
            AxisStep = TimeSpan.FromDays(1).Ticks;
            AxisUnit = TimeSpan.TicksPerDay;
            //初始化
            this.httpClient = httpClient;
            updateTimer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            PoseInfoVm_Now = new PoseInfoVm();
            PoseInfoVm_Week = new PoseInfoVm();
            PoseInfoVm_Month = new PoseInfoVm();

            GetAgedsAsync();
        }

        private void Loaded()
        {
            LogHelper.Debug("DataView Loaded.");
            //刷新老人信息
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
                    if (!Ageds.Any(x => x.Id == item.Id))
                    {
                        Ageds.Add(item);
                    }
                }
                //检查有无删减
                for (int i = Ageds.Count - 1; i >= 0; i--)
                {
                    bool isExit = false;
                    foreach (var item in ageds)
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

            //获取数据更新视图
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
                List<PoseInfo> poseinfoes_month = JsonConvert.DeserializeObject<List<PoseInfo>>(result);

                UpdateDayPieChart(poseinfoes_month);
                UpdateWeekPieChart(poseinfoes_month);
                UpdateMonthPieChart(poseinfoes_month);
                UpdateWeekStandLine(poseinfoes_month);
                UpdateWeekLieLine(poseinfoes_month);
                UpdateWeekSitLine(poseinfoes_month);
            }
        }
        //更新当前天的姿态信息饼图
        private void UpdateDayPieChart(IEnumerable<PoseInfo> poseInfos)
        {
            PoseInfoVm_Now.TimeDown = 0;
            PoseInfoVm_Now.TimeSit = 0;
            PoseInfoVm_Now.TimeLie = 0;
            PoseInfoVm_Now.TimeOther = 0;
            PoseInfoVm_Now.TimeStand = 0;
            var poseinfo_day = poseInfos.FirstOrDefault(x => x.Date == DateTime.Now.Date);
            if (poseinfo_day != null)
            {
                autoMapper.Map(poseinfo_day, PoseInfoVm_Now);
            }
        }
        //更新最近一周的姿态信息饼图
        private void UpdateWeekPieChart(IEnumerable<PoseInfo> poseInfos)
        {
            PoseInfoVm_Week.TimeDown += 0;
            PoseInfoVm_Week.TimeLie += 0;
            PoseInfoVm_Week.TimeOther += 0;
            PoseInfoVm_Week.TimeSit += 0;
            PoseInfoVm_Week.TimeStand += 0;
            var poseInfoWeek = poseInfos.Where(x => x.Date >= DateTime.Now.AddDays(-6).Date && x.Date <= DateTime.Now);

            foreach (var item in poseInfoWeek)
            {
                PoseInfoVm_Week.TimeDown += item.TimeDown;
                PoseInfoVm_Week.TimeLie += item.TimeLie;
                PoseInfoVm_Week.TimeOther += item.TimeOther;
                PoseInfoVm_Week.TimeSit += item.TimeSit;
                PoseInfoVm_Week.TimeStand += item.TimeStand;
            }
        }
        //更新最近一月的姿态信息饼图
        private void UpdateMonthPieChart(IEnumerable<PoseInfo> poseInfos)
        {
            PoseInfoVm_Month.TimeDown += 0;
            PoseInfoVm_Month.TimeLie += 0;
            PoseInfoVm_Month.TimeOther += 0;
            PoseInfoVm_Month.TimeSit += 0;
            PoseInfoVm_Month.TimeStand += 0;

            foreach (var item in poseInfos)
            {
                PoseInfoVm_Month.TimeDown += item.TimeDown;
                PoseInfoVm_Month.TimeLie += item.TimeLie;
                PoseInfoVm_Month.TimeOther += item.TimeOther;
                PoseInfoVm_Month.TimeSit += item.TimeSit;
                PoseInfoVm_Month.TimeStand += item.TimeStand;
            }
        }
        //更新最近一周站立信息线图
        private void UpdateWeekStandLine(IEnumerable<PoseInfo> poseInfos)
        {
            var pose_stand_week = poseInfos.Where(item => item.Date > DateTime.Now.AddDays(-7)).Select(pose => new MeasureModel()
            {
                DateTime = pose.Date,
                Value = pose.TimeStand
            });
            PoseInfoStandWeek.Clear();
            PoseInfoStandWeek.AddRange(pose_stand_week);
        }
        private void UpdateWeekLieLine(IEnumerable<PoseInfo> poseInfos)
        {
            var pose_lie_week = poseInfos.Where(item => item.Date > DateTime.Now.AddDays(-7)).Select(pose => new MeasureModel()
            {
                DateTime = pose.Date,
                Value = pose.TimeLie
            });
            PoseInfoLieWeek.Clear();
            PoseInfoLieWeek.AddRange(pose_lie_week);
        }
        private void UpdateWeekSitLine(IEnumerable<PoseInfo> poseInfos)
        {
            var pose_sit_week = poseInfos.Where(item => item.Date > DateTime.Now.AddDays(-7)).Select(pose => new MeasureModel()
            {
                DateTime = pose.Date,
                Value = pose.TimeSit
            });
            PoseInfoSitWeek.Clear();
            PoseInfoSitWeek.AddRange(pose_sit_week);
        }
        private void Unloaded()
        {
            LogHelper.Debug("DataView Unloaded.");
        }
    }
}
